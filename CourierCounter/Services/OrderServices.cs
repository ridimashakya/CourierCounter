using CourierCounter.Models.Entities;
using CourierCounter.Models;
using CourierCounter.Services.Interfaces;
using CourierCounter.Models.ApiModels.ApiResponse;
using CourierCounter.Data;
using Microsoft.EntityFrameworkCore;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.Enum;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CourierCounter.Location;

namespace CourierCounter.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMLPredictionService _mlPredictionService;
        private readonly INominatimGeocodingService _geocodingService;

        public OrderServices(ApplicationDbContext dbContext, IMLPredictionService mlPredictionService, INominatimGeocodingService geocodingService)
        {
            _dbContext = dbContext;
            _mlPredictionService = mlPredictionService;
            _geocodingService = geocodingService;
        }
        public async Task<ApiResponse<bool>> CreateOrder(OrdersViewModel data)
        {
            ApiResponse<bool> result;
            try
            {
                DeliveryOrderDataModel model = new DeliveryOrderDataModel
                {
                    Zone = (float)data.DeliveryZone,
                    DistanceInKm = data.DistanceInKm,
                    WeightInKg = data.WeightInKg,
                    UrgencyLevel = (float)data.UrgencyLevel
                };

                var wage = _mlPredictionService.PredictWage(model);

                Orders orderEntity = new Orders
                {
                    CustomerName = data.CustomerName,
                    CustomerEmail = data.CustomerEmail,
                    CustomerContactNumber = data.CustomerContactNumber,
                    //TrackingId = trackingId,
                    DeliveryAddress = data.DeliveryAddress,
                    DeliveryZone = data.DeliveryZone,
                    DistanceInKm = data.DistanceInKm,
                    WeightInKg = data.WeightInKg,
                    UrgencyLevel = data.UrgencyLevel,
                    Wage = Convert.ToDecimal(wage)
                };

                _dbContext.Orders.Add(orderEntity);
                await _dbContext.SaveChangesAsync();

                string trackingId = GenerateTrackingId(orderEntity.Id);
                orderEntity.TrackingId = trackingId;

                _dbContext.Orders.Update(orderEntity);
                await _dbContext.SaveChangesAsync();

                result = new ApiResponse<bool>(true, "New Order Added Successfully!");
            }
            catch (Exception)
            {

                result = new ApiResponse<bool>(false, "Error occurred while creating order");
            }
            return result;
        }

        public async Task<List<OrdersViewModel>> GetAllOrders()
        {
            List<OrdersViewModel> orders = new List<OrdersViewModel>();

            try
            {
                var ordersList = await _dbContext.Orders.ToListAsync();

                foreach (var order in ordersList)
                {
                    string displayAddress = order.DeliveryAddress;

                    if (!string.IsNullOrWhiteSpace(order.DeliveryAddress) && order.DeliveryAddress.Contains(','))
                    {
                        var coords = order.DeliveryAddress.Split(',');

                        if (coords.Length == 2 &&
                            double.TryParse(coords[0].Trim(), out double lat) &&
                            double.TryParse(coords[1].Trim(), out double lng))
                        {
                            try
                            {
                                displayAddress = await _geocodingService.ReverseGeocodeAsync(lat, lng);

                                // Optional: Wait 1 sec to avoid hitting rate limits (OpenStreetMap API limit is 1 req/sec)
                                await Task.Delay(1000);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Reverse geocoding failed for coordinates ({lat}, {lng}): {ex.Message}");
                            }
                        }
                    }

                    orders.Add(new OrdersViewModel
                    {
                        Id = order.Id,
                        TrackingId = order.TrackingId,
                        CustomerName = order.CustomerName,
                        CustomerEmail = order.CustomerEmail,
                        CustomerContactNumber = order.CustomerContactNumber,
                        DeliveryAddress = displayAddress,
                        DeliveryZone = order.DeliveryZone,
                        Status = order.Status,
                        DistanceInKm = order.DistanceInKm,
                        WeightInKg = order.WeightInKg,
                        UrgencyLevel = order.UrgencyLevel
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllOrders: {ex.Message}");
            }

            return orders;
        }


        public async Task<OrdersViewModel?> GetOrderById(int id)
        {
            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
                if (order == null)
                    return null;

                var workerName = (from workerOrder in _dbContext.WorkerOrder
                                join worker in _dbContext.AllWorkers on workerOrder.WorkerId equals worker.Id
                                where workerOrder.OrderId == id
                                select worker.FullName).FirstOrDefault() ?? "N/A";

                string displayAddress = order.DeliveryAddress;
                
                // If the address is in coordinate format, try to reverse geocode it
                if (double.TryParse(order.DeliveryAddress, out _) && order.DeliveryAddress.Contains(','))
                {
                    try
                    {
                        var coords = order.DeliveryAddress.Split(',');
                        if (coords.Length == 2 && 
                            double.TryParse(coords[0].Trim(), out double lat) && 
                            double.TryParse(coords[1].Trim(), out double lng))
                        {
                            displayAddress = await _geocodingService.ReverseGeocodeAsync(lat, lng);
                        }
                    }
                    catch (Exception ex)
                    {
                        // If reverse geocoding fails, keep the original coordinates
                        Console.WriteLine($"Reverse geocoding failed: {ex.Message}");
                    }
                }


                return new OrdersViewModel
                {
                    Id = order.Id,
                    TrackingId = order.TrackingId,
                    CustomerName = order.CustomerName,
                    CustomerEmail = order.CustomerEmail,
                    CustomerContactNumber = order.CustomerContactNumber,
                    DeliveryAddress = displayAddress,
                    DeliveryZone = order.DeliveryZone,
                    DistanceInKm = order.DistanceInKm,
                    WeightInKg = order.WeightInKg,
                    UrgencyLevel = order.UrgencyLevel,
                    Wage = order.Wage,
                    Status = order.Status,
                    WorkerName = workerName
                };
            }
            catch (Exception ex)
            {
                //return null;
            }
            return null;
        }

        public bool DeleteOrder(int id)
        {
            try
            {
                var order = _dbContext.Orders.Find(id);

                if (order == null)
                    return false;

                _dbContext.Orders.Remove(order);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<bool>> UpdateOrder(int id)
        {
            ApiResponse<bool> result;
            try
            {
                var existingOrderValue = (from order in _dbContext.Orders
                                          where order.Id == id
                                          select new OrdersViewModel
                                          {
                                              Id = order.Id,
                                              TrackingId = order.TrackingId,
                                              CustomerName = order.CustomerName,
                                              CustomerEmail = order.CustomerEmail,
                                              CustomerContactNumber = order.CustomerContactNumber,
                                              DeliveryAddress = order.DeliveryAddress,
                                              DeliveryZone = order.DeliveryZone,
                                              DistanceInKm = order.DistanceInKm,
                                              WeightInKg = order.WeightInKg,
                                              UrgencyLevel = order.UrgencyLevel,
                                              Wage = order.Wage,
                                              Status = order.Status
                                          }).FirstOrDefault();

                await _dbContext.SaveChangesAsync();
                result = new ApiResponse<bool>(true, "Order details listed successfully!");
            }
            catch (Exception ex)
            {
                result = new ApiResponse<bool>(false, "Order details cannot be listed");
            }
            return result;
        }

        public async Task<ApiResponse<bool>> UpdateOrder(OrdersViewModel data)
        {
            ApiResponse<bool> result;

            try
            {
                var order = await _dbContext.Orders.Where(x => x.Id == data.Id).FirstOrDefaultAsync();

                if (order == null)
                    return new ApiResponse<bool>(false, "Order Updated!");

                order.CustomerName = data.CustomerName;
                order.CustomerEmail = data.CustomerEmail;
                order.CustomerContactNumber = data.CustomerContactNumber;
                order.DeliveryAddress = data.DeliveryAddress;
                order.DistanceInKm = data.DistanceInKm;
                order.WeightInKg = data.WeightInKg;
                order.UrgencyLevel = data.UrgencyLevel;
                order.DeliveryZone = data.DeliveryZone;

                DeliveryOrderDataModel model = new DeliveryOrderDataModel
                {
                    Zone = (float)data.DeliveryZone,
                    DistanceInKm = data.DistanceInKm,
                    WeightInKg = data.WeightInKg,
                    UrgencyLevel = (float)data.UrgencyLevel
                };

                var wage = _mlPredictionService.PredictWage(model);
                order.Wage = (decimal)wage;

                _dbContext.Orders.Update(order);

                await _dbContext.SaveChangesAsync();
                result = new ApiResponse<bool>(true, "Order updated successfully!");
            }
            catch (Exception ex)
            {
                result = new ApiResponse<bool>(false, "Order cannot be updated");
            }
            return result;
        }

        private string GenerateTrackingId(int orderId)
        {
            string datePart = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            return $"{orderId}-{datePart}";
        }

        public async Task<ApiResponse<List<ForOrderViewModel>>> GetPendingSelectedOrders()
        {
            try
            {
                var workers = await (from o in _dbContext.Orders
                                     where !_dbContext.WorkerOrder.Any(wo => wo.OrderId == o.Id)
                                     select new ForOrderViewModel
                                     {
                                         DeliveryAddress = o.DeliveryAddress,
                                         TrackingId = o.TrackingId,
                                         DistanceInKm = o.DistanceInKm,
                                         WeightInKg = o.WeightInKg,
                                         UrgencyLevel = o.UrgencyLevel.ToString(),
                                         Wage = o.Wage
                                     }).ToListAsync();


                return new ApiResponse<List<ForOrderViewModel>>(true, "Pending selected order listing successfull!", workers);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ForOrderViewModel>>(false, "Pending selected order not listed.");
            }

           
        }

        public async Task<ApiResponse<List<ForOrderViewModel>>> GetInProgressSelectedOrders(string userId)
        {
            try
            {
                var workers = await (from worker in _dbContext.AllWorkers
                                     join wo in _dbContext.WorkerOrder on worker.Id equals wo.WorkerId
                                     join o in _dbContext.Orders on wo.OrderId equals o.Id
                                     where worker.UserId == userId
                                     && o.Status == OrderStatusEnum.InProgress
                                     select new ForOrderViewModel
                                     {
                                         DeliveryAddress = o.DeliveryAddress,
                                         TrackingId = o.TrackingId,
                                         DistanceInKm = o.DistanceInKm,
                                         WeightInKg = o.WeightInKg,
                                         UrgencyLevel = o.UrgencyLevel.ToString(),
                                         Wage = o.Wage
                                     }).ToListAsync();

                return new ApiResponse<List<ForOrderViewModel>>(true, "In progress orders listed sucessfully!", workers);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ForOrderViewModel>>(false, "In progress order not listed.");
            }
        }

        public async Task<ApiResponse<List<ForOrderViewModel>>> GetCompletedSelectedOrders(string userId)
        {
            try
            {
                var workersDeliveredOrders = await (
                    from worker in _dbContext.AllWorkers
                    where worker.UserId == userId
                    join wo in _dbContext.WorkerOrder on worker.Id equals wo.WorkerId
                    join o in _dbContext.Orders on wo.OrderId equals o.Id
                    where o.Status == OrderStatusEnum.Delivered
                    select new ForOrderViewModel
                    {
                        DeliveryAddress = o.DeliveryAddress,
                        TrackingId = o.TrackingId,
                        DistanceInKm = o.DistanceInKm,
                        WeightInKg = o.WeightInKg,
                        UrgencyLevel = o.UrgencyLevel.ToString(),
                        Wage = o.Wage
                    }
                ).ToListAsync();

                return new ApiResponse<List<ForOrderViewModel>>(true, "Completed orders listed successfully!", workersDeliveredOrders);
            }
            catch (Exception)
            {
                return new ApiResponse<List<ForOrderViewModel>>(false, "Completed orders not listed.");
            }
        }

        public async Task<ApiResponse<bool>> SavedSelectedOrders(WorkerOrdersViewModel data)
        {
            ApiResponse<bool> result;

            var orders = await _dbContext.Orders.Where(x => data.OrderId.Contains(x.Id)).ToListAsync();

            if (orders.Count != data.OrderId.Count)
            {
                result = new ApiResponse<bool>(false, "Some orders do not exist");
                return result;
            }

            try
            {
                foreach (var order in orders)
                {
                    order.Status = OrderStatusEnum.InProgress;
                }
                _dbContext.Orders.UpdateRange(orders);

                var workerOrdersList = data.OrderId.Select(orderId => new WorkerOrders
                {
                    WorkerId = data.WorkerId,
                    OrderId = orderId,
                    CreatedDate = DateTime.Now
                }).ToList();

                _dbContext.WorkerOrder.AddRange(workerOrdersList);

                await _dbContext.SaveChangesAsync();

                result = new ApiResponse<bool>(true, "Selected orders saved successfully!");
            }
            catch (Exception)
            {
                result = new ApiResponse<bool>(false, "Error saving the selected orders.");
            }
            return result;
        }

        public async Task<ApiResponse<bool>> SavedCompletedOrders(WorkerOrdersViewModel data)
        {
            ApiResponse<bool> result;

            var orders = await _dbContext.Orders
                .Where(x => data.OrderId.Contains(x.Id))
                .ToListAsync();

            if (orders.Count != data.OrderId.Count)
            {
                result = new ApiResponse<bool>(false, "Some orders do not exist");
                return result;
            }

            try
            {
                foreach (var order in orders)
                {
                    order.Status = OrderStatusEnum.Delivered;
                }
                _dbContext.Orders.UpdateRange(orders);

                foreach (var orderId in data.OrderId)
                {
                    var existingWorkerOrder = await _dbContext.WorkerOrder
                        .FirstOrDefaultAsync(wo => wo.OrderId == orderId && wo.WorkerId == data.WorkerId);

                    if (existingWorkerOrder == null)
                    {
                        var newWorkerOrder = new WorkerOrders
                        {
                            OrderId = orderId,
                            WorkerId = data.WorkerId,
                            CreatedDate = DateTime.UtcNow
                        };
                        _dbContext.WorkerOrder.Add(newWorkerOrder);
                    }
                }

                await _dbContext.SaveChangesAsync();

                result = new ApiResponse<bool>(true, "Selected orders marked as completed successfully!");
            }
            catch (Exception)
            {
                result = new ApiResponse<bool>(false, "Error marking the selected orders as completed.");
            }

            return result;
        }
    }
}


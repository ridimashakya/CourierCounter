using CourierCounter.Models.Entities;
using CourierCounter.Models;
using CourierCounter.Services.Interfaces;
using CourierCounter.Models.ApiModels.ApiResponse;
using CourierCounter.Data;
using Microsoft.EntityFrameworkCore;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.Enum;

namespace CourierCounter.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMLPredictionService _mlPredictionService;

        public OrderServices(ApplicationDbContext dbContext, IMLPredictionService mlPredictionService)
        {
            _dbContext = dbContext;
            _mlPredictionService = mlPredictionService;
        }
        public async Task<ApiResponse<bool>> CreateOrder(OrdersViewModel data)
        {
            ApiResponse<bool> result;
            try
            {
                var trackingId = GenerateTrackingId();

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
                    TrackingId = trackingId,
                    DeliveryAddress = data.DeliveryAddress,
                    DeliveryZone = data.DeliveryZone,
                    DistanceInKm = data.DistanceInKm,
                    WeightInKg = data.WeightInKg,
                    UrgencyLevel = data.UrgencyLevel,
                    Wage = Convert.ToDecimal(wage)
                };

                _dbContext.Orders.Add(orderEntity);
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
                orders = await (from order in _dbContext.Orders
                                select new OrdersViewModel
                                {
                                    Id = order.Id,
                                    TrackingId = order.TrackingId,
                                    CustomerName = order.CustomerName,
                                    CustomerEmail = order.CustomerEmail,
                                    CustomerContactNumber = order.CustomerContactNumber,
                                    DeliveryAddress = order.DeliveryAddress,
                                    DeliveryZone = order.DeliveryZone,
                                    Status = order.Status,
                                    DistanceInKm = order.DistanceInKm,
                                    WeightInKg = order.WeightInKg,
                                    UrgencyLevel = order.UrgencyLevel
                                }).ToListAsync();
            }
            catch (Exception)
            {

            }

            return orders;
        }

        public OrdersViewModel? GetOrderById(int id)
        {
            try
            {
                var workerName = (from workerOrder in _dbContext.WorkerOrder
                                  join worker in _dbContext.AllWorkers on workerOrder.WorkerId equals worker.Id
                                  where workerOrder.OrderId == id
                                  select worker.FullName).FirstOrDefault() ?? "N/A"; 

               var orderValues = (from order in _dbContext.Orders
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
                                   Status = order.Status,
                                   WorkerName = workerName
                               }).FirstOrDefault();
                return orderValues;
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

        private string GenerateTrackingId()
        {
            string datePart = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            string randomPart = GenerateRandomString(5).ToUpper();

            return $"{randomPart}-{datePart}";
        }

        private string GenerateRandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<ApiResponse<List<ForOrderViewModel>>> GetPendingSelectedOrders()
        {
            //ApiResponse<List<ForOrderViewModel>> response = null;
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
            //ApiResponse<List<ForOrderViewModel>> response = null;

            try
            {
                var workers = await (from worker in _dbContext.AllWorkers
                                     join wo in _dbContext.WorkerOrder on worker.Id equals wo.Id
                                     join o in _dbContext.Orders on wo.Id equals o.Id
                                     where o.Status == OrderStatusEnum.Delivered
                                     select new ForOrderViewModel
                                     {
                                         DeliveryAddress = o.DeliveryAddress,
                                         TrackingId = o.TrackingId,
                                         DistanceInKm = o.DistanceInKm,
                                         WeightInKg = o.WeightInKg,
                                         UrgencyLevel = o.UrgencyLevel.ToString(),
                                         Wage = o.Wage
                                     }).ToListAsync();

                return new ApiResponse<List<ForOrderViewModel>>(true, "Completed orders listed sucessfully!", workers);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ForOrderViewModel>>(false, "Completed orders not listed.");
            }
        }

        public async Task<ApiResponse<bool>> SavedSelectedOrders(WorkerOrdersViewModel data)
        {
            ApiResponse<bool> result;

            var createDate = DateTime.Now;

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
                    CreatedDate = createDate
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
    }
}


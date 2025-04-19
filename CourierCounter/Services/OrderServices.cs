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

        public OrderServices(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiResponse<bool>> CreateOrder(OrdersViewModel data)
        {
            ApiResponse<bool> result;
            try
            {
                var trackingId = GenerateTrackingId();

                Orders orderEntity = new Orders
                {
                    CustomerName = data.CustomerName,
                    CustomerEmail = data.CustomerEmail,
                    CustomerContactNumber = data.CustomerContactNumber,
                    TrackingId = trackingId,
                    DeliveryAddress = data.DeliveryAddress,
                    DeliveryZone = data.DeliveryZone
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
                                    Status = order.Status
                                }).ToListAsync();
            }
            catch (Exception)
            {

            }

            return orders;
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
            ApiResponse<List<ForOrderViewModel>> response = null;
            try
            {
                var workers = await (from o in _dbContext.Orders
                                     where !_dbContext.WorkerOrder.Any(wo => wo.OrderId == o.Id)
                                     select new ForOrderViewModel
                                     {
                                         DeliveryAddress = o.DeliveryAddress,
                                         TrackingId = o.TrackingId
                                     }).ToListAsync();


                response = new ApiResponse<List<ForOrderViewModel>>(true, "Pending selected order listing successfull!", workers);
            }
            catch (Exception ex)
            {
                response = new ApiResponse<List<ForOrderViewModel>>(false, "Pending selected order not listed.");
            }
            return response;
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
                                         TrackingId = o.TrackingId
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
            ApiResponse<List<ForOrderViewModel>> response = null;

            try
            {
                var workers = await (from worker in _dbContext.AllWorkers
                                     join wo in _dbContext.WorkerOrder on worker.Id equals wo.Id
                                     join o in _dbContext.Orders on wo.Id equals o.Id
                                     where o.Status == OrderStatusEnum.Delivered
                                     select new ForOrderViewModel
                                     {
                                         DeliveryAddress = o.DeliveryAddress,
                                         TrackingId = o.TrackingId
                                     }).ToListAsync();

                response = new ApiResponse<List<ForOrderViewModel>>(true, "Completed orders listed sucessfully!", workers);
            }
            catch (Exception ex)
            {
                response = new ApiResponse<List<ForOrderViewModel>>(false, "Completed orders not listed.");
            }
            return response;
        }

        public async Task<ApiResponse<bool>> SavedSelectedOrders(WorkerOrdersViewModel data)
        {
            ApiResponse<bool> result;

            var createDate = DateTime.Now;

            var order = await _dbContext.Orders.Where(x => x.Id == data.OrderId).FirstOrDefaultAsync();
            if (order == null)
            {
                result = new ApiResponse<bool>(false, "Order doesn't exist");
                return result;
            }

            order.Status = OrderStatusEnum.InProgress;
            _dbContext.Orders.Update(order);

            try
            {
                WorkerOrders workerOrders = new WorkerOrders
                {
                    WorkerId = data.WorkerId,
                    OrderId = data.OrderId,
                    CreatedDate = createDate
                };

                _dbContext.WorkerOrder.Add(workerOrders);
                await _dbContext.SaveChangesAsync();

                result = new ApiResponse<bool>(true, "Selceted orders saved successfully!");
            }
            catch (Exception)
            {
                result = new ApiResponse<bool>(false, "Error saving the salected orders.");
            }

            return result;
        }
    }
}


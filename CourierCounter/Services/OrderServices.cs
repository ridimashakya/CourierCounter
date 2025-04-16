using CourierCounter.Models.Entities;
using CourierCounter.Models;
using CourierCounter.Services.Interfaces;
using CourierCounter.Models.ApiModels.ApiResponse;
using CourierCounter.Data;
using Microsoft.EntityFrameworkCore;

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
                              DeliveryZone = order.DeliveryZone
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
            string randomPart = GenerateRandomString(5).ToUpper(); // 5-letter random code

            return $"{randomPart}-{datePart}";
        }

        private string GenerateRandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

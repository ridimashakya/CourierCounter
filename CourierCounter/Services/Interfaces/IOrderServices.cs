using CourierCounter.Models;
using CourierCounter.Models.ApiModels.ApiResponse;
using CourierCounter.Models.Entities;

namespace CourierCounter.Services.Interfaces
{
    public interface IOrderServices
    {
        Task<ApiResponse<bool>> CreateOrder(OrdersViewModel data);
        Task<List<OrdersViewModel>> GetAllOrders();
    }
}

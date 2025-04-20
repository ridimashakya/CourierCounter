using CourierCounter.Models;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.ApiModels.ApiResponse;
using CourierCounter.Models.Entities;
using CourierCounter.Models.Enum;

namespace CourierCounter.Services.Interfaces
{
    public interface IOrderServices
    {
        Task<ApiResponse<bool>> CreateOrder(OrdersViewModel data);
        Task<List<OrdersViewModel>> GetAllOrders();
        bool DeleteOrder(int id);
        Task<ApiResponse<bool>> SavedSelectedOrders(WorkerOrdersViewModel data);
        Task<ApiResponse<List<ForOrderViewModel>>> GetPendingSelectedOrders();
        Task<ApiResponse<List<ForOrderViewModel>>> GetInProgressSelectedOrders(string userId);
        Task<ApiResponse<List<ForOrderViewModel>>> GetCompletedSelectedOrders(string userId);
    }
}

using CourierCounter.Models;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.ApiModels.ApiResponse;
using CourierCounter.Models.Entities;
using CourierCounter.Models.Enum;
using Microsoft.AspNetCore.Mvc;

namespace CourierCounter.Services.Interfaces
{
    public interface IOrderServices
    {
        Task<ApiResponse<bool>> CreateOrder(OrdersViewModel data);
        Task<List<OrdersViewModel>> GetAllOrders();
        Task<OrdersViewModel?> GetOrderById(int id);
        bool DeleteOrder(int id);
        Task<ApiResponse<bool>> UpdateOrder(int id);
        Task<ApiResponse<bool>> UpdateOrder(OrdersViewModel data);
        Task<ApiResponse<bool>> SavedSelectedOrders(WorkerOrdersViewModel data);
        Task<ApiResponse<List<ForOrderViewModel>>> GetPendingSelectedOrders();
        Task<ApiResponse<List<ForOrderViewModel>>> GetInProgressSelectedOrders(string userId);
        Task<ApiResponse<bool>> SavedCompletedOrders(WorkerOrdersViewModel data);
        Task<ApiResponse<List<ForOrderViewModel>>> GetCompletedSelectedOrders(string userId);
    }
}

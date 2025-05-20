using CourierCounter.Models.Dashboard;

namespace CourierCounter.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<OrdersCountViewModel> GetOrderDetail();
        Task<WorkersCountViewModel> GetWorkerDetail();
    }
}

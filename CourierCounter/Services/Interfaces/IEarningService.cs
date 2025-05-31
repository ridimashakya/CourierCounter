using CourierCounter.Models;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.ApiModels.ApiResponse;

namespace CourierCounter.Services.Interfaces
{
    public interface IEarningService
    {
        Task<ApiResponse<EarningViewModel>>GetTodayEarnings(string userId);
        Task<ApiResponse<List<EarningHistoryViewModel>>> GetEarningHistory(string userId);
        Task<PayoutsViewModel> GetPayouts();
        Task<bool> MarkAsPaid(int workerId);
    }
}

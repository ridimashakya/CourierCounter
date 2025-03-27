using CourierCounter.Models.ApiModels;
using CourierCounter.Models.ApiModels.ApiResponse;

namespace CourierCounter.Services.Interfaces
{
    public interface IWorkerServices
    {
        Task<ApiResponse<bool>> CreateWorker(RegistrationViewModel data);
        ApiResponse<bool> UpdateWorker(RegistrationViewModel data);
    }
}

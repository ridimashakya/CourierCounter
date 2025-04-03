using CourierCounter.Models;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.ApiModels.ApiResponse;
using CourierCounter.Models.Enum;

namespace CourierCounter.Services.Interfaces
{
    public interface IWorkerServices
    {
        Task<ApiResponse<bool>> CreateWorker(RegistrationViewModel data);
        ApiResponse<bool> UpdateWorker(RegistrationViewModel data);
        Worker GetWorkerById(int id);
        bool UpdateStatusById(int id, StatusEnum status);
        List<Worker> GetAllWorker(StatusEnum? status = null);
    }
}

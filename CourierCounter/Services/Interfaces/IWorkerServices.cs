using CourierCounter.Models;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.ApiModels.ApiResponse;
using CourierCounter.Models.Enum;
using Microsoft.AspNetCore.Mvc;

namespace CourierCounter.Services.Interfaces
{
    public interface IWorkerServices
    {
        Task<ApiResponse<bool>> CreateWorker(RegistrationViewModel data);
        Task UpdateWorkerLocationAsync(int workerId, double lat, double lon);
        Worker GetWorkerById(int id);
        Task<WorkerDetailsViewModel> GetWorkerByUserId(string userId);
        bool UpdateStatusById(int id, StatusEnum status);
        Task<List<Worker>> GetAllWorkerAsync(StatusEnum? status = null);
        public bool DeleteWorker(int id);
    }
}

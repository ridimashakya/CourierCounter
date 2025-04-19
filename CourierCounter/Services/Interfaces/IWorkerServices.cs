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
        Worker GetWorkerById(int id);
        bool UpdateStatusById(int id, StatusEnum status);
        List<Worker> GetAllWorker(StatusEnum? status = null);
        public bool DeleteWorker(int id);
    }
}

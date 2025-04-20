using CourierCounter.Models.ApiModels;
using CourierCounter.Models.ApiModels.ApiResponse;

namespace CourierCounter.Services.Interfaces
{
    public interface ILoginServices
    {
        Task<ApiResponse<string>> Login(LoginViewModel data);
    }
}

using CourierCounter.Models;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.ApiModels.ApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace CourierCounter.Services.Interfaces
{
    public interface ILoginServices
    {
        Task<ApiResponse<string>> Login(LoginViewModel data);
        Task<bool> AdminLogin(AdminLoginModel data);
        Task<bool> AdminLogout();
    }
}

using CourierCounter.Data;
using CourierCounter.Models;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.ApiModels.ApiResponse;
using CourierCounter.Models.Entities;
using CourierCounter.Models.Enum;
using CourierCounter.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace CourierCounter.Services
{
    public class LoginServices : ILoginServices
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public LoginServices(SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbContext)
        {
            _signInManager = signInManager;
            _dbContext = dbContext;
        }
        public async Task<ApiResponse<bool>> Login(LoginViewModel data)
        {
            ApiResponse<bool> result;

            var res = await _signInManager.PasswordSignInAsync(data.Email, data.Password, false, false);

            if (!res.Succeeded)
                return new ApiResponse<bool>(false, "Login Failed! Incorrect credentials.");
            else
            {
                var workerStatus = _dbContext.AllWorkers.Where(x => x.Email == data.Email).Select(x => x.Status).FirstOrDefault();

                if (workerStatus != StatusEnum.Approved)
                {
                    string message = workerStatus == StatusEnum.Pending
                                     ? "Login Failed! Your profile is still in the process of verification"
                                     : "Login Failed! Yor profile has been rejected. Please contact the Admin.";
                    return new ApiResponse<bool>(false, message);
                }


                result = new ApiResponse<bool>(true, "Login Successfull! You are now verified worker.");
            }
            return result;
        }
    }
}

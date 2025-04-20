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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public LoginServices(SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _signInManager = signInManager;
            _dbContext = dbContext;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        public async Task<ApiResponse<string>> Login(LoginViewModel data)
        {
            ApiResponse<string> result;

            var res = await _signInManager.PasswordSignInAsync(data.Email, data.Password, false, false);

            if (!res.Succeeded)
                return new ApiResponse<string>(false, "Login Failed! Incorrect credentials.");
            else
            {
                var workerStatus = _dbContext.AllWorkers.Where(x => x.Email == data.Email).Select(x => x.Status).FirstOrDefault();

                if (workerStatus != StatusEnum.Approved)
                {
                    string message = workerStatus == StatusEnum.Pending
                                     ? "Login Failed! Your profile is still in the process of verification"
                                     : "Login Failed! Yor profile has been rejected. Please contact the Admin.";
                    return new ApiResponse<string>(false, message);
                }

                var user = await _userManager.FindByEmailAsync(data.Email);
                var token = _tokenService.GenerateToken(user);

                result = new ApiResponse<string>(true, "Login Successfull! You are now verified worker.", user.Id);
            }
            return result;
        }
    }
}

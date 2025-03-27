using CourierCounter.Data;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.ApiModels.ApiResponse;
using CourierCounter.Models.Entities;
using CourierCounter.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;

namespace CourierCounter.Services
{
    public class LoginServices : ILoginServices
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginServices(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public async Task<ApiResponse<bool>> Login(LoginViewModel data)
        {
            ApiResponse<bool> result;

            var res = await _signInManager.PasswordSignInAsync(data.Email, data.Password, false, false);
            
            if(!res.Succeeded)
                return new ApiResponse<bool>(false, "Login Failed!");
            else
            {
                result = new ApiResponse<bool>(true, "Login Successfull!");
            }

            return result;
        }
    }
}

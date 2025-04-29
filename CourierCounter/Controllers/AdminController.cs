using CourierCounter.Models;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.Entities;
using CourierCounter.Services;
using CourierCounter.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourierCounter.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly ILoginServices _loginServices;

        public AdminController(ILoginServices loginServices)
        {
            _loginServices = loginServices;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginModel data)
        {
            var result = await _loginServices.AdminLogin(data);
            if (!result)
                return View();

            return Redirect("/");
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _loginServices.AdminLogout();
            if (!result)
                return Redirect("/");

            return Redirect("/admin/login");
        }
    }
}

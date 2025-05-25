using CourierCounter.Models;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.Entities;
using CourierCounter.Services;
using CourierCounter.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourierCounter.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly ILoginServices _loginServices;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ILoginServices loginServices, UserManager<ApplicationUser> userManager)
        {
            _loginServices = loginServices;
            _userManager = userManager;
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
        [Route("login")]
        public async Task<IActionResult> Login(AdminLoginModel data)
        {
            var isValid = await _loginServices.AdminLogin(data); 
            if (!isValid)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View("Index");
            }

            var user = await _userManager.FindByEmailAsync(data.Email);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Redirect("/");
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _loginServices.AdminLogout();

            if (!result)
                return Redirect("/");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/admin/login");
        }
    }
}

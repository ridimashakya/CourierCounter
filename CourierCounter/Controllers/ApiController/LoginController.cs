using AspNetCoreGeneratedDocument;
using CourierCounter.Models.ApiModels;
using CourierCounter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourierCounter.Controllers.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly ILoginServices _loginServices;

        public LoginController(ILoginServices loginServices)
        {
            _loginServices = loginServices;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginWorker([FromBody] LoginViewModel data)
        {
            var result = await _loginServices.Login(data);
            return Ok(result);
        }
    }
}

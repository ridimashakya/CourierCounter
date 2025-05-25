using CourierCounter.Models;
using CourierCounter.Models.ApiModels;
using CourierCounter.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourierCounter.Controllers.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerDetailsController : Controller
    {
        private readonly IWorkerServices _workerServices;

        public WorkerDetailsController(IWorkerServices workerServices)
        {
            _workerServices = workerServices;
        }

        [Route("getworkerdetails")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetWorkerDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid token or missing user ID.");

            var result = await _workerServices.GetWorkerByUserId(userId);
            return Ok(result);
        }
    }
}

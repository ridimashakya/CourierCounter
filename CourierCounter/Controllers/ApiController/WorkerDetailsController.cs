using CourierCounter.Models;
using CourierCounter.Models.ApiModels;
using CourierCounter.Services.Interfaces;
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
        public async Task<IActionResult> GetWorkerDetails()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _workerServices.GetWorkerByUserId(userId);
            return Ok(result);
        }
    }
}

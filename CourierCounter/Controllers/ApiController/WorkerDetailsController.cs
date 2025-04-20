using CourierCounter.Models;
using CourierCounter.Models.ApiModels;
using CourierCounter.Services.Interfaces;
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

        
        [Route("getworkerdetails/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetWorkerDetails(string userId)
        {
            //string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            userId = userId == "null" ? "92c5b40f-8a3f-4f6d-8eb1-d08eb03747f9" : userId;

            var result = await _workerServices.GetWorkerByUserId(userId);
            return Ok(result);
        }
    }
}

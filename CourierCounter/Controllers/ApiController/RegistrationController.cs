using CourierCounter.Data;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.ApiModels.ApiResponse;
using CourierCounter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourierCounter.Controllers.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IWorkerServices _workerServices;

        public RegistrationController(IWorkerServices workerServices)
        {
            _workerServices = workerServices;
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> RegisterWorker([FromBody] RegistrationViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _workerServices.CreateWorker(data);
            return Ok(result);
        }
    }
}

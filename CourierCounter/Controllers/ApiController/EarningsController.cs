using Azure;
using Azure.Core;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.ApiModels.ApiResponse;
using CourierCounter.Services;
using CourierCounter.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourierCounter.Controllers.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EarningsController : Controller
    {
        private readonly IEarningService _earningService;

        public EarningsController(IEarningService earningService)
        {
            _earningService = earningService;
        }

        [Route("calculate")]
        [HttpGet]
        public async Task<IActionResult> GetTodayEarnings()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse<string>(false, "Invalid user token"));
           
            var result = await _earningService.GetTodayEarnings(userId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Route("history")]
        [HttpGet]
        public async Task<IActionResult> GetEarningHistory()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse<string>(false, "Invalid user token"));

            var result = await _earningService.GetEarningHistory(userId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

    }
}

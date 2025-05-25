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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [Route("allOrders")]
        [HttpGet]
        public async Task<IActionResult> OrderList()
        {
            var result = await _orderServices.GetAllOrders();
            return Ok(result);
        }

        [Route("saveselectedorders")]
        [HttpPost]
        public async Task<IActionResult> SavedSelectedOrders(WorkerOrdersViewModel data)
        {
            var result = await _orderServices.SavedSelectedOrders(data);
            return Ok(result);
        }

        [Route("pendingorders")]
        [HttpGet]
        public async Task<IActionResult> PendingOrders()
        {
            var result = await _orderServices.GetPendingSelectedOrders();
            return Ok(result);
        }

        [Route("inprogressorders")]
        [HttpGet]
        public async Task<IActionResult> InProgressOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token");

            var result = await _orderServices.GetInProgressSelectedOrders(userId);
            return Ok(result);
        }

        [Route("deliveredorders")]
        [HttpGet]
        public async Task<IActionResult> DeliveredOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token");

            var result = await _orderServices.GetCompletedSelectedOrders(userId);
            return Ok(result);
        }

    }
}

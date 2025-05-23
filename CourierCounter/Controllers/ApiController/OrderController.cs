﻿using CourierCounter.Models;
using CourierCounter.Models.ApiModels;
using CourierCounter.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourierCounter.Controllers.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
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

        [Route("inprogressorders/{userId}")]
        [HttpGet]
        public async Task<IActionResult> InProgressOrders(string userId)
        {
            //string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            userId = userId == "null" ? "92c5b40f-8a3f-4f6d-8eb1-d08eb03747f9" : userId;

            var result = await _orderServices.GetInProgressSelectedOrders(userId);
            return Ok(result);
        }

        [Route("deliveredorders/{userId}")]
        [HttpGet]
        public async Task<IActionResult> DeliveredOrders(string userId)
        {
            //string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            userId = userId == "null" ? "92c5b40f-8a3f-4f6d-8eb1-d08eb03747f9" : userId;

            var result = await _orderServices.GetCompletedSelectedOrders(userId);
            return Ok(result);
        }

    }
}

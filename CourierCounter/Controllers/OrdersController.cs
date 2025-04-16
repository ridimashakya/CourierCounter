using CourierCounter.Models;
using CourierCounter.Models.ApiModels.ApiResponse;
using CourierCounter.Models.Entities;
using CourierCounter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourierCounter.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderServices _orderServices;

        public OrdersController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpGet]
        [Route("orders")]
        public async Task<IActionResult> Index()
        {
            var result = await _orderServices.GetAllOrders();
            return View(result);
        }

        public IActionResult AddOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(OrdersViewModel data)
        {
            var result = await _orderServices.CreateOrder(data);
            ModelState.Clear();
            return View();
        }

    }
}


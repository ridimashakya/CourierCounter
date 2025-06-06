using CourierCounter.Models;
using CourierCounter.Models.ApiModels.ApiResponse;
using CourierCounter.Models.Entities;
using CourierCounter.Services;
using CourierCounter.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourierCounter.Controllers
{
    [Authorize]
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

        [HttpGet]
        public IActionResult AddOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(OrdersViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _orderServices.CreateOrder(data);
            ModelState.Clear();
            return View();
        }

        [Route("order/delete/{id}")]
        public bool DeleteOrderById(int id)
        {
            return _orderServices.DeleteOrder(id);
        }

        [Route("order/details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderServices.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpGet]
        [Route("order/edit/{id}")]
        public async Task<IActionResult> UpdateOrder(int id)
        {
            var result = await _orderServices.GetOrderById(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [HttpPost]
        [Route("order/edit/{id}")]
        public async Task<IActionResult> UpdateOrder(OrdersViewModel data)
        {
            var result = await _orderServices.UpdateOrder(data);
            return View( );
        }
    }
}


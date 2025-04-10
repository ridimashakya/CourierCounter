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

        [Route("orders")]
        public IActionResult Index()
        {
            //Orders order = _orderServices.GetOrders();
            return View();
        }
    }
}

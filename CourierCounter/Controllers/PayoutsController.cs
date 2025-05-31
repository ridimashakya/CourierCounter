using CourierCounter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourierCounter.Controllers
{
    [Route("payouts")]
    public class PayoutsController : Controller
    {
        private readonly IEarningService _earningService;

        public PayoutsController(IEarningService earningService)
        {
            _earningService = earningService;
        }
        
        public async Task<IActionResult> Index()
        {
            var result = await _earningService.GetPayouts();
            return View(result);
        }

        [HttpPost]
        [Route("markaspaid")]
        public async Task<IActionResult> MarkAsPaid(int workerId)
        {
            var success = await _earningService.MarkAsPaid(workerId);
            if (success)
                return Json(new { success = true, message = "Marked as paid." });

            return Json(new { success = false, message = "Failed to mark as paid." });
        }
    }
}

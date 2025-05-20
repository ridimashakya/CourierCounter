using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CourierCounter.Models;
using Microsoft.AspNetCore.Authorization;
using CourierCounter.Services.Interfaces;

namespace CourierCounter.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IDashboardService _dashboardService;

    public HomeController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var orderCountDetails = await _dashboardService.GetOrderDetail();
        var workerCountDetails = await _dashboardService.GetWorkerDetail();


        var model = new DashboardViewModel
        {
            OrdersCount = orderCountDetails,
            WorkersCount = workerCountDetails
        };

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

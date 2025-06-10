using CourierCounter.Models;
using CourierCounter.Models.Enum;
using CourierCounter.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourierCounter.Controllers
{
    [Authorize]
    public class WorkersController : Controller
    {
        private readonly IWorkerServices _workerServices;

        public WorkersController(IWorkerServices workerServices)
        {
            _workerServices = workerServices;
        }

        [Route("workers")]
        public async Task<IActionResult> Index(StatusEnum? status = null)
        {
            WorkerViewModel workers = new WorkerViewModel
            {
                Status = status,
                WorkerList = await _workerServices.GetAllWorkerAsync(status)
            };

            return View(workers);
        }

        [Route("worker/details/{id}")]
        public IActionResult Details(int id)
        {
            Worker worker = _workerServices.GetWorkerById(id);
            return View(worker);
        }

        [Route("worker/approvereject")]
        public IActionResult ApproveReject(int id, StatusEnum status)
        {
            bool result = _workerServices.UpdateStatusById(id, status);

            return Redirect("details/" + id);
        }

        [Route("worker/delete/{id}")]
        public bool DeleteWorker(int id)
        {
            return _workerServices.DeleteWorker(id);
        }
    }
}

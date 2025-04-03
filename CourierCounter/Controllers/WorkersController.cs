using CourierCounter.Models;
using CourierCounter.Models.Enum;
using CourierCounter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourierCounter.Controllers
{
    public class WorkersController : Controller
    {
        private readonly IWorkerServices _workerServices;

        public WorkersController(IWorkerServices workerServices)
        {
            _workerServices = workerServices;
        }

        [Route("workers")]
        public IActionResult Index(StatusEnum? status = null)
        {
            WorkerViewModel workers = new WorkerViewModel();
            workers.Status = status;
            workers.WorkerList = _workerServices.GetAllWorker(status);
            return View(workers);
        }

        public IWorkerServices Get_workerServices()
        {
            return _workerServices;
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

        //[Route("filterqueryworkers")]
        //public List<WorkerViewModel> GetFilteredWorkerByStatus([FromQuery] StatusEnum? status = null)
        //{
        //    List<WorkerViewModel> workers = _workerServices.GetAllWorker(status);
        //    return workers;
        //}

    }
}

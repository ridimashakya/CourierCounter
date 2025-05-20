using CourierCounter.Data;
using CourierCounter.Models.Dashboard;
using CourierCounter.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Identity.Client;

namespace CourierCounter.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _dbContext;

        public DashboardService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrdersCountViewModel> GetOrderDetail()
        {
            try
            {
                var totalOrder = await _dbContext.Orders.CountAsync();

                var pendingOrderCount = await (from order in _dbContext.Orders
                                               where order.Status == Models.Enum.OrderStatusEnum.Pending
                                               select order).CountAsync();

                var inProgressOrderCount = await (from order in _dbContext.Orders
                                                 where order.Status == Models.Enum.OrderStatusEnum.InProgress
                                                 select order).CountAsync();

                var deliveredOrderCount = await (from order in _dbContext.Orders
                                                 where order.Status == Models.Enum.OrderStatusEnum.Delivered
                                                 select order).CountAsync();

                return new OrdersCountViewModel
                {
                    TotalOrder = totalOrder,
                    PendingOrderCount = pendingOrderCount,
                    InProgressOrderCount = inProgressOrderCount,
                    DeliveredOrderCount = deliveredOrderCount
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<WorkersCountViewModel> GetWorkerDetail()
        {
            try
            {
                var totalWorker = await _dbContext.AllWorkers.CountAsync();

                var pendinglWorker = await (from worker in _dbContext.AllWorkers
                                         where worker.Status == Models.Enum.StatusEnum.Pending
                                         select worker).CountAsync();

                var approvedWorker = await (from worker in _dbContext.AllWorkers
                                            where worker.Status == Models.Enum.StatusEnum.Approved
                                            select worker).CountAsync();

                var rejectedWorker = await (from worker in _dbContext.AllWorkers
                                            where worker.Status == Models.Enum.StatusEnum.Rejected
                                            select worker).CountAsync();

                return new WorkersCountViewModel
                {
                    TotalWorker = totalWorker,
                    PendingWorker = pendinglWorker,
                    ApprovedWorker = approvedWorker,
                    RejectedWorker = rejectedWorker
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}

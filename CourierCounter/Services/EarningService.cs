//using AspNetCoreGeneratedDocument;
//using CourierCounter.Data;
//using CourierCounter.Models;
//using CourierCounter.Models.ApiModels;
//using CourierCounter.Models.ApiModels.ApiResponse;
//using CourierCounter.Models.Entities;
//using CourierCounter.Models.Enum;
//using CourierCounter.Services.Interfaces;
//using Microsoft.EntityFrameworkCore;

//namespace CourierCounter.Services
//{
//    public class EarningService : IEarningService
//    {
//        private readonly ApplicationDbContext _dbContext;

//        public EarningService(ApplicationDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public async Task<ApiResponse<EarningViewModel>> GetTodayEarnings(string userId)
//        {
//            var today = DateTime.Now.Date;

//            var worker = await _dbContext.AllWorkers.FirstOrDefaultAsync(w => w.UserId == userId);
//            if (worker == null)
//                return new ApiResponse<EarningViewModel>(false, "Worker not found");

//            // Confirm worker exists in DB
//            var workerExists = await _dbContext.AllWorkers.AnyAsync(w => w.Id == worker.Id);
//            if (!workerExists)
//            {
//                return new ApiResponse<EarningViewModel>(false, $"Worker with Id {worker.Id} not found in database.");
//            }

//            var workerOrders = await _dbContext.WorkerOrder
//                .Where(wo => wo.WorkerId == worker.Id)
//                .ToListAsync();

//            var todayOrders = workerOrders.Where(wo => wo.CreatedDate.Date == today).ToList();

//            var dailyEarning = await _dbContext.DailyEarnings
//                .FirstOrDefaultAsync(de => de.WorkerId == worker.Id);

//            if (!todayOrders.Any())
//            {
//                if (dailyEarning != null && dailyEarning.TotalWage > 0)
//                {
//                    var history = new EarningHistory
//                    {
//                        WorkerId = worker.Id,
//                        Date = dailyEarning.CreatedDate.Date,
//                        TotalWages = dailyEarning.TotalWage
//                    };

//                    try
//                    {
//                        await _dbContext.EarningHistories.AddAsync(history);

//                        dailyEarning.TotalWage = 0;
//                        dailyEarning.CreatedDate = today;

//                        await _dbContext.SaveChangesAsync();
//                    }
//                    catch (Exception ex)
//                    {
//                        return new ApiResponse<EarningViewModel>(false, "Error saving earnings history: " + ex.Message);
//                    }
//                }

//                return new ApiResponse<EarningViewModel>(true, "No earnings for today", new EarningViewModel
//                {
//                    TotalWages = 0,
//                    Date = today.ToString("yyyy-MM-dd"),
//                    Orders = new List<OrderWageDetail>()
//                });
//            }

//            var orderIds = todayOrders.Select(wo => wo.OrderId).ToList();
//            var deliveredOrders = await _dbContext.Orders
//                .Where(o => orderIds.Contains(o.Id) && o.Status == OrderStatusEnum.Delivered)
//                .ToListAsync();

//            decimal totalWage = 0;
//            var orderDetails = new List<OrderWageDetail>();

//            foreach (var order in deliveredOrders)
//            {
//                totalWage += order.Wage;
//                orderDetails.Add(new OrderWageDetail
//                {
//                    OrderId = order.Id,
//                    Wage = order.Wage
//                });
//            }

//            if (dailyEarning == null)
//            {
//                dailyEarning = new DailyEarning
//                {
//                    WorkerId = worker.Id,
//                    CreatedDate = today,
//                    TotalWage = totalWage
//                };
//                _dbContext.DailyEarnings.Add(dailyEarning);
//            }
//            else
//            {
//                dailyEarning.CreatedDate = today;
//                dailyEarning.TotalWage = totalWage;
//                _dbContext.DailyEarnings.Update(dailyEarning);
//            }

//            await _dbContext.SaveChangesAsync();

//            return new ApiResponse<EarningViewModel>(true, "Earnings fetched successfully", new EarningViewModel
//            {
//                TotalWages = totalWage,
//                Date = today.ToString("yyyy-MM-dd"),
//                Orders = orderDetails
//            });
//        }

//        public async Task<ApiResponse<List<EarningHistoryViewModel>>> GetEarningHistory(string userId)
//        {
//            var worker = await _dbContext.AllWorkers.FirstOrDefaultAsync(w => w.UserId == userId);
//            if (worker == null)
//                return new ApiResponse<List<EarningHistoryViewModel>>(false, "Worker not found");

//            var history = await _dbContext.EarningHistories
//                .Where(eh => eh.WorkerId == worker.Id)
//                .OrderByDescending(eh => eh.Date)
//                .Select(eh => new EarningHistoryViewModel
//                {
//                    Date = eh.Date.ToString("yyyy-MM-dd"),
//                    TotalWage = eh.TotalWages
//                })
//                .ToListAsync();

//            return new ApiResponse<List<EarningHistoryViewModel>>(true, "Earning history fetched successfully", history);
//        }

//        public async Task<PayoutsViewModel> GetPayouts()
//        {
//            var payouts = await _dbContext.DailyEarnings
//                .OrderByDescending(e => e.CreatedDate)
//                .ToListAsync();

//            var workerIds = payouts.Select(e => e.WorkerId).Distinct().ToList();

//            var workers = await _dbContext.AllWorkers
//                .Where(w => workerIds.Contains(w.Id))
//                .ToDictionaryAsync(w => w.Id);

//            var viewModel = new PayoutsViewModel
//            {
//                WorkerPayoutList = payouts.Select(e =>
//                {
//                    workers.TryGetValue(e.WorkerId, out var worker);

//                    return new WorkerPayout
//                    {
//                        Id = e.Id,
//                        WorkerId = e.WorkerId,
//                        WorkerName = worker?.FullName ?? "Unknown",
//                        TotalWage = e.TotalWage,
//                        isPaid = e.isPaid,
//                        ProfileImagePath = worker?.ProfileImagePath,
//                        PaidDate = e.PaidDate
//                    };
//                }).ToList()
//            };

//            return viewModel;
//        }

//        public async Task<bool> MarkAsPaid(int workerId)
//        {
//            var earning = await _dbContext.DailyEarnings
//                .Where(e => e.WorkerId == workerId && !e.isPaid)
//                .OrderByDescending(e => e.CreatedDate)
//                .FirstOrDefaultAsync();

//            if (earning == null)
//                return false;

//            earning.isPaid = true;
//            earning.PaidDate = DateTime.Now;

//            await _dbContext.SaveChangesAsync();

//            return true;
//        }

//    }
//}

using AspNetCoreGeneratedDocument;
using CourierCounter.Data;
using CourierCounter.Models;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.ApiModels.ApiResponse;
using CourierCounter.Models.Entities;
using CourierCounter.Models.Enum;
using CourierCounter.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourierCounter.Services
{
    public class EarningService : IEarningService
    {
        private readonly ApplicationDbContext _dbContext;

        public EarningService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get today's earnings for a worker, archive previous day earnings if needed
        public async Task<ApiResponse<EarningViewModel>> GetTodayEarnings(string userId)
        {
            var today = DateTime.Now.Date;

            var worker = await _dbContext.AllWorkers.FirstOrDefaultAsync(w => w.UserId == userId);
            if (worker == null)
                return new ApiResponse<EarningViewModel>(false, "Worker not found");

            // Get all orders assigned to this worker
            var workerOrders = await _dbContext.WorkerOrder
                .Where(wo => wo.WorkerId == worker.Id)
                .ToListAsync();

            // Filter orders assigned today
            var todayOrders = workerOrders.Where(wo => wo.CreatedDate.Date == today).ToList();

            // Get the daily earning record for this worker
            var dailyEarning = await _dbContext.DailyEarnings
                .FirstOrDefaultAsync(de => de.WorkerId == worker.Id);

            // Archive previous day earnings if dailyEarning is from an earlier date
            if (dailyEarning != null && dailyEarning.CreatedDate.Date < today)
            {
                if (dailyEarning.TotalWage > 0)
                {
                    var history = new EarningHistory
                    {
                        WorkerId = worker.Id,
                        Date = dailyEarning.CreatedDate.Date,
                        TotalWages = dailyEarning.TotalWage
                    };

                    _dbContext.EarningHistories.Add(history);
                }

                // Reset daily earning for today
                dailyEarning.TotalWage = 0;
                dailyEarning.CreatedDate = today;

                await _dbContext.SaveChangesAsync();
            }

            // If no orders today, return current daily wage (could be zero)
            if (!todayOrders.Any())
            {
                return new ApiResponse<EarningViewModel>(true, "No earnings for today", new EarningViewModel
                {
                    TotalWages = dailyEarning?.TotalWage ?? 0,
                    Date = today.ToString("yyyy-MM-dd"),
                    Orders = new List<OrderWageDetail>()
                });
            }

            // Get delivered orders among todayOrders
            var orderIds = todayOrders.Select(wo => wo.OrderId).ToList();
            var deliveredOrders = await _dbContext.Orders
                .Where(o => orderIds.Contains(o.Id) && o.Status == OrderStatusEnum.Delivered)
                .ToListAsync();

            decimal totalWage = 0;
            var orderDetails = new List<OrderWageDetail>();

            foreach (var order in deliveredOrders)
            {
                totalWage += order.Wage;
                orderDetails.Add(new OrderWageDetail
                {
                    OrderId = order.Id,
                    Wage = order.Wage
                });
            }

            if (dailyEarning == null)
            {
                // Create new daily earning record for today
                dailyEarning = new DailyEarning
                {
                    WorkerId = worker.Id,
                    CreatedDate = today,
                    TotalWage = totalWage
                };
                _dbContext.DailyEarnings.Add(dailyEarning);
            }
            else
            {
                // Update existing daily earning record for today
                dailyEarning.CreatedDate = today;
                dailyEarning.TotalWage = totalWage;
                _dbContext.DailyEarnings.Update(dailyEarning);
            }

            await _dbContext.SaveChangesAsync();

            return new ApiResponse<EarningViewModel>(true, "Earnings fetched successfully", new EarningViewModel
            {
                TotalWages = totalWage,
                Date = today.ToString("yyyy-MM-dd"),
                Orders = orderDetails
            });
        }

        // Get all archived earning histories of a worker
        public async Task<ApiResponse<List<EarningHistoryViewModel>>> GetEarningHistory(string userId)
        {
            var worker = await _dbContext.AllWorkers.FirstOrDefaultAsync(w => w.UserId == userId);
            if (worker == null)
                return new ApiResponse<List<EarningHistoryViewModel>>(false, "Worker not found");

            var history = await _dbContext.EarningHistories
                .Where(eh => eh.WorkerId == worker.Id)
                .OrderByDescending(eh => eh.Date)
                .Select(eh => new EarningHistoryViewModel
                {
                    Date = eh.Date.ToString("yyyy-MM-dd"),
                    TotalWage = eh.TotalWages
                })
                .ToListAsync();

            return new ApiResponse<List<EarningHistoryViewModel>>(true, "Earning history fetched successfully", history);
        }

        // Get payouts listing (daily earnings and paid status)
        public async Task<PayoutsViewModel> GetPayouts()
        {
            var payouts = await _dbContext.DailyEarnings
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();

            var workerIds = payouts.Select(e => e.WorkerId).Distinct().ToList();

            var workers = await _dbContext.AllWorkers
                .Where(w => workerIds.Contains(w.Id))
                .ToDictionaryAsync(w => w.Id);

            var viewModel = new PayoutsViewModel
            {
                WorkerPayoutList = payouts.Select(e =>
                {
                    workers.TryGetValue(e.WorkerId, out var worker);

                    return new WorkerPayout
                    {
                        Id = e.Id,
                        WorkerId = e.WorkerId,
                        WorkerName = worker?.FullName ?? "Unknown",
                        TotalWage = e.TotalWage,
                        isPaid = e.isPaid,
                        ProfileImagePath = worker?.ProfileImagePath,
                        PaidDate = e.PaidDate
                    };
                }).ToList()
            };

            return viewModel;
        }

        // Mark daily earning as paid for a worker
        public async Task<bool> MarkAsPaid(int workerId)
        {
            var earning = await _dbContext.DailyEarnings
                .Where(e => e.WorkerId == workerId && !e.isPaid)
                .OrderByDescending(e => e.CreatedDate)
                .FirstOrDefaultAsync();

            if (earning == null)
                return false;

            earning.isPaid = true;
            earning.PaidDate = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}


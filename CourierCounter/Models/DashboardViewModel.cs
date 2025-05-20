using CourierCounter.Models.Dashboard;

namespace CourierCounter.Models
{
    public class DashboardViewModel
    {
        public OrdersCountViewModel OrdersCount { get; set; }
        public WorkersCountViewModel WorkersCount { get; set; }
        public OrdersViewModel TrackingId { get; set; }
    }
}

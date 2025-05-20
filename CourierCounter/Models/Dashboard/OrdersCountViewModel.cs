using CourierCounter.Models.Enum;

namespace CourierCounter.Models.Dashboard
{
    public class OrdersCountViewModel
    {
        public int TotalOrder { get; set; }
        public int PendingOrderCount { get; set; }
        public int InProgressOrderCount { get; set; }
        public int DeliveredOrderCount { get; set; }
    }
}

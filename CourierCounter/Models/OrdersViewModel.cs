using CourierCounter.Models.Enum;

namespace CourierCounter.Models
{
    public class OrdersViewModel
    {
        public int TrackingId { get; set; }
        public required string CustomerName { get; set; }
        public required string CustomerContactNumber { get; set; }
        public required string CustomerEmail { get; set; }
        public required string DeliveryAddress { get; set; }
        public DeliveryZoneEnum DeliveryZone { get; set; }
    }
}



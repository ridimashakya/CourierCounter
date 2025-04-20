using CourierCounter.Models.Enum;

namespace CourierCounter.Models
{
    public class OrdersViewModel
    {
        public int Id { get; set; }
        public required string TrackingId { get; set; }
        public required string CustomerName { get; set; }
        public required string CustomerContactNumber { get; set; }
        public required string CustomerEmail { get; set; }
        public required string DeliveryAddress { get; set; }
        public float DistanceInKm { get; set; }
        public float WeightInKg { get; set; }
        public decimal Wage { get; set; }
        public UrgencyLevelEnum UrgencyLevel { get; set; }
        public DeliveryZoneEnum DeliveryZone { get; set; }
        public OrderStatusEnum Status { get; set; }
    }
}



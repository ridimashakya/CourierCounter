using CourierCounter.Models.Enum;

namespace CourierCounter.Models.Entities
{
    public class Orders
    {
        public int Id { get; set; }
        public string TrackingId { get; set; } = string.Empty;
        public required string CustomerName { get; set; }
        public required string CustomerContactNumber { get; set; }
        public required string CustomerEmail { get; set; }
        public required string DeliveryAddress { get; set; }
        public double DeliveryLatitude { get; set; }
        public double DeliveryLongitude { get; set; }
        public DeliveryZoneEnum DeliveryZone { get; set; }
        public float DistanceInKm { get; set; } 
        public float WeightInKg { get; set; } 
        public UrgencyLevelEnum UrgencyLevel { get; set; } 
        public decimal Wage { get; set; }
        public OrderStatusEnum Status { get; set; }
    }
}

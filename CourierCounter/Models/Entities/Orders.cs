using CourierCounter.Models.Enum;

namespace CourierCounter.Models.Entities
{
    public class Orders
    {
        public int Id { get; set; }
        public required string TrackingId { get; set; }
        public required string CustomerName { get; set; }
        public required string CustomerContactNumber { get; set; }
        public required string CustomerEmail { get; set; }
        public required string DeliveryAddress { get; set; }
        public DeliveryZoneEnum DeliveryZone { get; set; }
        public OrderStatus Status { get; set; }
    }
}

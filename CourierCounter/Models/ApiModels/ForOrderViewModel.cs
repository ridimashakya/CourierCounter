using CourierCounter.Models.Enum;

namespace CourierCounter.Models.ApiModels
{
    public class ForOrderViewModel
    {
        public required string DeliveryAddress { get; set; }
        public required string TrackingId { get; set; }
    }
}

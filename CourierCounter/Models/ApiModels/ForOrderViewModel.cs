using CourierCounter.Models.Enum;

namespace CourierCounter.Models.ApiModels
{
    public class ForOrderViewModel
    {
        public required string DeliveryAddress { get; set; }
        public string? TrackingId { get; set; }
        public required float DistanceInKm { get; set; }
        public required float WeightInKg { get; set; }
        public required string UrgencyLevel { get; set; }
        public required decimal Wage { get; set; }
    }
}

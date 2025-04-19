using CourierCounter.Models.Enum;
using Microsoft.ML.Data;

namespace CourierCounter.Models
{
    public class DeliveryOrderDataModel
    {
        public float Zone { get; set; }
        public float DistanceInKm { get; set; }
        public float WeightInKg { get; set; }
        public float UrgencyLevel { get; set; }

        [ColumnName("Label")]
        public float Wage { get; set; }
    }
}

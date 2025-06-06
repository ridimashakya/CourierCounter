using Microsoft.ML.Data;

namespace CourierCounter.Models.Entities
{
    public class WageTrainingData
    {
        public int Id { get; set; }
        public int Zone { get; set; }
        public float DistanceInKm { get; set; }
        public float WeightInKg { get; set; }
        public float UrgencyLevel { get; set; }

        [ColumnName("Label")]
        public float Wage { get; set; }
    }
}

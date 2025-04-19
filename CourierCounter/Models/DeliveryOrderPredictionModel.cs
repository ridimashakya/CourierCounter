using Microsoft.ML.Data;

namespace CourierCounter.Models
{
    public class DeliveryOrderPredictionModel
    {
        [ColumnName("Score")]
        public float PredictedWage { get; set; }
    }
}

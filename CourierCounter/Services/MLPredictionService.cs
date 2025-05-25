//using CourierCounter.Models;
//using CourierCounter.Models.Enum;
//using CourierCounter.Services.Interfaces;
//using Microsoft.ML;

//namespace CourierCounter.Services
//{
//    public class MLPredictionService : IMLPredictionService
//    {
//        public float PredictWage(DeliveryOrderDataModel newOrder)
//        {
//            try
//            {
//                //initialize MLContext
//                var mlContext = new MLContext();

//                #region Data Set
//                var data = new List<DeliveryOrderDataModel>

//                {
//                    new DeliveryOrderDataModel { Zone = 1, DistanceInKm = 3, WeightInKg = 1, UrgencyLevel = 1, Wage = 100 },
//                    new DeliveryOrderDataModel { Zone = 2, DistanceInKm = 6, WeightInKg = 2, UrgencyLevel = 2, Wage = 160 },
//                    new DeliveryOrderDataModel { Zone = 3, DistanceInKm = 4, WeightInKg = 1.5f, UrgencyLevel = 3, Wage = 200 },
//                    new DeliveryOrderDataModel { Zone = 4, DistanceInKm = 15, WeightInKg = 5, UrgencyLevel = 1, Wage = 300 },
//                    new DeliveryOrderDataModel { Zone = 1, DistanceInKm = 1, WeightInKg = 0.5f, UrgencyLevel = 1, Wage = 80 },
//                    new DeliveryOrderDataModel { Zone = 2, DistanceInKm = 8, WeightInKg = 3, UrgencyLevel = 3, Wage = 220 },
//                    new DeliveryOrderDataModel { Zone = 3, DistanceInKm = 2, WeightInKg = 1, UrgencyLevel = 2, Wage = 150 },
//                    new DeliveryOrderDataModel { Zone = 4, DistanceInKm = 20, WeightInKg = 4, UrgencyLevel = 3, Wage = 350 },
//                    new DeliveryOrderDataModel { Zone = 1, DistanceInKm = 5, WeightInKg = 2, UrgencyLevel = 1, Wage = 120 },
//                    new DeliveryOrderDataModel { Zone = 2, DistanceInKm = 7, WeightInKg = 3, UrgencyLevel = 2, Wage = 180 },
//                    new DeliveryOrderDataModel { Zone = 3, DistanceInKm = 10, WeightInKg = 5, UrgencyLevel = 1, Wage = 260 },
//                    new DeliveryOrderDataModel { Zone = 4, DistanceInKm = 25, WeightInKg = 6, UrgencyLevel = 3, Wage = 400 },

//                    new DeliveryOrderDataModel { Zone = 1, DistanceInKm = 1, WeightInKg = 1, UrgencyLevel = 1, Wage = 100 },
//                    new DeliveryOrderDataModel { Zone = 2, DistanceInKm = 6, WeightInKg = 2, UrgencyLevel = 2, Wage = 160 },
//                    new DeliveryOrderDataModel { Zone = 3, DistanceInKm = 4, WeightInKg = 1.5f, UrgencyLevel = 3, Wage = 200 },
//                    new DeliveryOrderDataModel { Zone = 4, DistanceInKm = 15, WeightInKg = 5, UrgencyLevel = 1, Wage = 300 },
//                    new DeliveryOrderDataModel { Zone = 1, DistanceInKm = 1, WeightInKg = 0.5f, UrgencyLevel = 1, Wage = 80 },
//                    new DeliveryOrderDataModel { Zone = 2, DistanceInKm = 8, WeightInKg = 3, UrgencyLevel = 3, Wage = 220 },
//                    new DeliveryOrderDataModel { Zone = 3, DistanceInKm = 2, WeightInKg = 1, UrgencyLevel = 2, Wage = 150 },
//                    new DeliveryOrderDataModel { Zone = 4, DistanceInKm = 20, WeightInKg = 4, UrgencyLevel = 3, Wage = 350 },
//                    new DeliveryOrderDataModel { Zone = 1, DistanceInKm = 5, WeightInKg = 2, UrgencyLevel = 1, Wage = 120 },
//                    new DeliveryOrderDataModel { Zone = 2, DistanceInKm = 7, WeightInKg = 3, UrgencyLevel = 2, Wage = 180 },
//                    new DeliveryOrderDataModel { Zone = 3, DistanceInKm = 10, WeightInKg = 5, UrgencyLevel = 1, Wage = 260 },
//                    new DeliveryOrderDataModel { Zone = 4, DistanceInKm = 25, WeightInKg = 6, UrgencyLevel = 3, Wage = 400 },

//                    new DeliveryOrderDataModel { Zone = 1, DistanceInKm = 3, WeightInKg = 1, UrgencyLevel = 1, Wage = 100 },
//                    new DeliveryOrderDataModel { Zone = 2, DistanceInKm = 6, WeightInKg = 2, UrgencyLevel = 2, Wage = 160 },
//                    new DeliveryOrderDataModel { Zone = 3, DistanceInKm = 4, WeightInKg = 1.5f, UrgencyLevel = 3, Wage = 200 },
//                    new DeliveryOrderDataModel { Zone = 4, DistanceInKm = 15, WeightInKg = 5, UrgencyLevel = 1, Wage = 300 },
//                    new DeliveryOrderDataModel { Zone = 1, DistanceInKm = 1, WeightInKg = 0.5f, UrgencyLevel = 1, Wage = 80 },
//                    new DeliveryOrderDataModel { Zone = 2, DistanceInKm = 8, WeightInKg = 3, UrgencyLevel = 3, Wage = 220 },
//                    new DeliveryOrderDataModel { Zone = 3, DistanceInKm = 2, WeightInKg = 1, UrgencyLevel = 2, Wage = 150 },
//                    new DeliveryOrderDataModel { Zone = 4, DistanceInKm = 20, WeightInKg = 4, UrgencyLevel = 3, Wage = 350 },
//                    new DeliveryOrderDataModel { Zone = 1, DistanceInKm = 5, WeightInKg = 2, UrgencyLevel = 1, Wage = 120 },
//                    new DeliveryOrderDataModel { Zone = 2, DistanceInKm = 7, WeightInKg = 3, UrgencyLevel = 2, Wage = 180 },
//                    new DeliveryOrderDataModel { Zone = 3, DistanceInKm = 10, WeightInKg = 5, UrgencyLevel = 1, Wage = 260 },
//                    new DeliveryOrderDataModel { Zone = 4, DistanceInKm = 25, WeightInKg = 6, UrgencyLevel = 3, Wage = 400 },
//                };
//                #endregion

//                var trainingData = mlContext.Data.LoadFromEnumerable(data);

//                //optimization method
//                var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Wage")
//            .Append(mlContext.Transforms.Concatenate("Features", "Zone", "DistanceInKm", "WeightInKg", "UrgencyLevel"))
//            .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features"));

//                var model = pipeline.Fit(trainingData);

//                var predictionEngine = mlContext.Model.CreatePredictionEngine<DeliveryOrderDataModel, DeliveryOrderPredictionModel>(model);
//                var prediction = predictionEngine.Predict(newOrder);

//                return prediction.PredictedWage;
//            }
//            catch (Exception ex)
//            {

//            }

//            return 0;
//        }
//    }
//}

using CourierCounter.Models;
using CourierCounter.Models.Entities;
using CourierCounter.Models.Enum;
using CourierCounter.Services.Interfaces;
using Microsoft.ML;
using System.Runtime.InteropServices;

namespace CourierCounter.Services
{
    public class MLPredictionService : IMLPredictionService
    {
        public float PredictWage(DeliveryOrderDataModel newOrder)
        {
            {
                const float baseFee = 60f;
                const float baseKm = 3f;
                const float extraPerKm = 10f;

                const float zoneBonusMultiplier = 20f;

                float extradistanceCost = newOrder.DistanceInKm > baseKm ? (newOrder.DistanceInKm - baseKm) * extraPerKm : 0f;

                float urgencyCost = newOrder.UrgencyLevel switch
                {
                    (float)UrgencyLevelEnum.Low => 0f,
                    (float)UrgencyLevelEnum.Medium => 10f,
                    (float)UrgencyLevelEnum.High => 20f,
                    _ => 0f
                };

                float weightFee = newOrder.WeightInKg switch
                {
                    <= 0 => 0f,
                    <= 10 => 20f,
                    <= 20 => 40f,
                    <= 30 => 60f,
                    <= 40 => 80f,
                    <= 50 => 100f,
                    _ => 120f
                };

                float outOfValleyZoneBonus = newOrder.Zone == 4 ? zoneBonusMultiplier : 0f;
                float totalWage = baseFee + extradistanceCost + weightFee + urgencyCost + outOfValleyZoneBonus;

                return totalWage;
            }
        }
    }
}

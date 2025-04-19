using CourierCounter.Models;
using CourierCounter.Models.Enum;
using CourierCounter.Services.Interfaces;
using Microsoft.ML;

namespace CourierCounter.Services
{
    public class MLPredictionService : IMLPredictionService
    {
        public float PredictWage(DeliveryOrderDataModel newOrder)
        {
            try
            {
                //initialize MLContext
                var mlContext = new MLContext();

                #region Generate Random Data
                var random = new Random();
                var data = new List<DeliveryOrderDataModel>();

                for (int i = 0; i < 100; i++)
                {
                    var zone = (DeliveryZoneEnum)random.Next(1, 5);
                    var distance = random.Next(1, 30);
                    var weight = (float)(random.NextDouble() * 5 + 0.5);
                    var urgency = (UrgencyLevelEnum)random.Next(1, 4);
                    var wage = 50 + (int)zone * 20 + distance * 10 + weight * 15 + (int)urgency * 25;

                    data.Add(new DeliveryOrderDataModel { Zone = (float)zone, DistanceInKm = distance, WeightInKg = weight, UrgencyLevel = (float)urgency, Wage = wage });
                }
                #endregion

                var trainingData = mlContext.Data.LoadFromEnumerable(data);

                var pipeline = mlContext.Transforms.Concatenate("Features", "Zone", "DistanceInKm", "WeightInKg", "UrgencyLevel")
           .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features"));

                var model = pipeline.Fit(trainingData);

                var predictionEngine = mlContext.Model.CreatePredictionEngine<DeliveryOrderDataModel, DeliveryOrderPredictionModel>(model);
                var prediction = predictionEngine.Predict(newOrder);

                return prediction.PredictedWage;
            }
            catch (Exception ex)
            {

            }

            return 0;
        }
    }
}

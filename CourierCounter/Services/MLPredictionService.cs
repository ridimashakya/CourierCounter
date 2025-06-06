using CourierCounter.Data;
using CourierCounter.Models;
using CourierCounter.Models.Entities;
using CourierCounter.Models.Enum;
using CourierCounter.Services.Interfaces;
using Microsoft.ML;

namespace CourierCounter.Services
{
    public class MLPredictionService : IMLPredictionService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _modelPath = Path.Combine(AppContext.BaseDirectory, "MLModels", "WageModel.zip");
        private MLContext _mlContext;
        private ITransformer _mlModel;
        private PredictionEngine<DeliveryOrderDataModel, DeliveryOrderPredictionModel> _predictionEngine;

        public MLPredictionService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _mlContext = new MLContext();

            if (File.Exists(_modelPath))
            {
                using var stream = new FileStream(_modelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                _mlModel = _mlContext.Model.Load(stream, out var modelInputSchema);
                _predictionEngine = _mlContext.Model.CreatePredictionEngine<DeliveryOrderDataModel, DeliveryOrderPredictionModel>(_mlModel);
            }
        }

        public float PredictWage(DeliveryOrderDataModel newOrder)
        {
            float staticWage = CalculateStaticWage(newOrder);
            SaveTrainingData(newOrder, staticWage);

            // Only predict using ML if a model exists AND you have enough training data
            bool hasEnoughTrainingData = _dbContext.WageTrainingDataset.Count() >= 10;

            if (_predictionEngine != null && hasEnoughTrainingData)
            {
                try
                {
                    var prediction = _predictionEngine.Predict(newOrder);
                    if (prediction.PredictedWage > 0)   
                        return prediction.PredictedWage;
                }
                catch
                {
                    // fallback silently
                }
            }

            return staticWage;
        }

        private float CalculateStaticWage(DeliveryOrderDataModel order)
        {
            const float baseFee = 60f;
            const float baseKm = 3f;
            const float extraPerKm = 10f;
            const float zoneBonusMultiplier = 20f;

            float extradistanceCost = order.DistanceInKm > baseKm ? (order.DistanceInKm - baseKm) * extraPerKm : 0f;

            float urgencyCost = order.UrgencyLevel switch
            {
                (float)UrgencyLevelEnum.Low => 0f,
                (float)UrgencyLevelEnum.High => 20f,
                _ => 0f
            };

            float weightFee = order.WeightInKg switch
            {
                <= 0 => 0f,
                <= 10 => 20f,
                <= 20 => 40f,
                <= 30 => 60f,
                <= 40 => 80f,
                <= 50 => 100f,
                _ => 120f
            };

            float outOfValleyZoneBonus = order.Zone == 4 ? zoneBonusMultiplier : 0f;
            float totalWage = baseFee + extradistanceCost + weightFee + urgencyCost + outOfValleyZoneBonus;

            return totalWage;
        }

        private async void SaveTrainingData(DeliveryOrderDataModel order, float wage)
        {
            try
            {
                var trainingEntry = new WageTrainingData
                {
                    Zone = (int)order.Zone,
                    DistanceInKm = order.DistanceInKm,
                    WeightInKg = order.WeightInKg,
                    UrgencyLevel = order.UrgencyLevel,
                    Wage = wage
                };

                _dbContext.WageTrainingDataset.Add(trainingEntry);
                _dbContext.SaveChanges();
            }
            catch
            {
            }
        }

        public void TrainModel()
        {
            var data = _dbContext.WageTrainingDataset.ToList();

            if (data == null || data.Count == 0)
                throw new InvalidOperationException("No training data available to train model.");

            //Converts List<WageTrainingData> into an IDataView, the core data structure for ML.NET.
            var trainingData = _mlContext.Data.LoadFromEnumerable(data);

            var pipeline = _mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(WageTrainingData.Wage))
                .Append(_mlContext.Transforms.Concatenate("Features", nameof(WageTrainingData.Zone), nameof(WageTrainingData.DistanceInKm), nameof(WageTrainingData.WeightInKg), nameof(WageTrainingData.UrgencyLevel)))
                .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features"));

            //Train the Model
            var model = pipeline.Fit(trainingData);

            //Save the Model to a File
            var modelDir = Path.GetDirectoryName(_modelPath);
            if (!Directory.Exists(modelDir))
                Directory.CreateDirectory(modelDir);

            using var fs = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write);
            _mlContext.Model.Save(model, trainingData.Schema, fs);

            _mlModel = model;

            //Prepares a reusable engine that can accept new delivery orders and predict wages.
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<DeliveryOrderDataModel, DeliveryOrderPredictionModel>(model);
        }
    }
}

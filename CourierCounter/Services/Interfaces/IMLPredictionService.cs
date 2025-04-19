using CourierCounter.Models;

namespace CourierCounter.Services.Interfaces
{
    public interface IMLPredictionService
    {
        float PredictWage(DeliveryOrderDataModel input);
    }
}

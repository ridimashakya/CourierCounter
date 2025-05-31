namespace CourierCounter.Models.ApiModels
{
    public class WorkerOrdersViewModel
    {
        public int WorkerId { get; set; }
        public List<int> OrderId { get; set; }
    }
}

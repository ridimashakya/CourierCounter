//namespace CourierCounter.Models.ApiModels
//{
//    public class WorkerOrdersViewModel
//    {
//        public int WorkerId { get; set; }
//        public int OrderId { get; set; }
//    }
//}

namespace CourierCounter.Models.ApiModels
{
    public class WorkerOrdersViewModel
    {
        public int WorkerId { get; set; }
        public List<int> OrderId { get; set; }
    }
}

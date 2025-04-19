namespace CourierCounter.Models.Entities
{
    public class WorkerOrders
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public int OrderId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}

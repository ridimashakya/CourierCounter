namespace CourierCounter.Models.Entities
{
    public class DailyEarning
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalWage { get; set; }
        public bool isPaid { get; set; }
        public DateTime? PaidDate { get; set; }
    }
}
    
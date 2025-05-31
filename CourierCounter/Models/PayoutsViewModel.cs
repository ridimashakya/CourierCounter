namespace CourierCounter.Models
{
    public class PayoutsViewModel
    {
        public string? CreatedDate { get; set; }
        public List<WorkerPayout> WorkerPayoutList { get; set; }
    }

    public class WorkerPayout
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public string WorkerName { get; set; }
        public decimal TotalWage { get; set; }
        public bool isPaid { get; set; } = false;

        public string? ProfileImagePath { get; set; }
        public DateTime? PaidDate { get; set; }
    }
}

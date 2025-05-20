namespace CourierCounter.Models.Dashboard
{
    public class WorkersCountViewModel
    {
        public int TotalWorker { get; set; }
        public int ApprovedWorker { get; set; }
        public int PendingWorker { get; set; }
        public int RejectedWorker { get; set; }
    }
}

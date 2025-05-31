namespace CourierCounter.Models.ApiModels
{
    public class EarningViewModel
    {
        public decimal TotalWages { get; set; }
        public string Date { get; set; }
        public List<OrderWageDetail> Orders { get; set; }
    }
    
    public class OrderWageDetail
    {
        public int OrderId { get; set; }
        public decimal Wage { get; set; }
    }
}

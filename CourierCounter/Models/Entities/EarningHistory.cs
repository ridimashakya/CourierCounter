using CourierCounter.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierCounter.Models.Entities
{
    public class EarningHistory
    {
        public int Id { get; set; }

        public int WorkerId { get; set; }

        [ForeignKey(nameof(WorkerId))]
        public Workers Worker { get; set; }

        public decimal TotalWages { get; set; }
        public DateTime Date { get; set; }
    }
}

using CourierCounter.Models.Enum;

namespace CourierCounter.Models
{
    public class WorkerViewModel
    {
        public StatusEnum? Status { get; set; }
        public List<Worker> WorkerList { get; set; }
    }

    public class Worker
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string HomeAddress { get; set; }

        public string VehicleRegistrationNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string NationalIdNumber { get; set; }
        public StatusEnum Status { get; set; }
    }
}


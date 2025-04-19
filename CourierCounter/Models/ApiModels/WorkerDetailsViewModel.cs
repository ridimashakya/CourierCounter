using CourierCounter.Models.Enum;

namespace CourierCounter.Models.ApiModels
{
    public class WorkerDetailsViewModel
    {
        //public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string HomeAddress { get; set; }

        public string VehicleRegistrationNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string NationalIdNumber { get; set; }
        //public StatusEnum Status { get; set; }
    }
}

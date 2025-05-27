using CourierCounter.Models.Enum;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CourierCounter.Models.ApiModels
{
    public class RegistrationViewModel
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ContactNumber { get; set; }
        public required string HomeAddress { get; set; }

        public required string VehicleRegistrationNumber { get; set; }
        public required string LicenseNumber { get; set; }
        public required string NationalIdNumber { get; set; }

        public required IFormFile VehicleRegistrationNumberImage { get; set; }
        public required IFormFile LicenseNumberImage { get; set; }
        public required IFormFile NationalIdNumberImage { get; set; }
    }                   
}

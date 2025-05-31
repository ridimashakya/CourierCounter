using CourierCounter.Models.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierCounter.Models.Entities
{
    [Table("AllWorkers")]
    public class Workers
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ContactNumber { get; set; }
        public required string HomeAddress { get; set; }

        public required string VehicleRegistrationNumber { get; set; }
        public required string LicenseNumber { get; set; }
        public required string NationalIdNumber { get; set; }

        public StatusEnum Status { get; set; }

        public string ProfileImagePath { get; set; } = string.Empty;

        public string VehicleRegistrationNumberImagePath { get; set; } = string.Empty;
        public string LicenseNumberImagePath { get; set; } = string.Empty;
        public string NationalIdNumberImagePath { get; set; } = string.Empty;
    }
}

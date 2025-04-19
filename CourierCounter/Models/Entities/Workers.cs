using CourierCounter.Models.Enum;

namespace CourierCounter.Models.Entities
{
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
    }
}

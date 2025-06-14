﻿using CourierCounter.Models.Enum;
using System.ComponentModel.DataAnnotations.Schema;

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

        public string VehicleRegistrationNumberImagePath { get; set; } = string.Empty;
        public string LicenseNumberImagePath { get; set; } = string.Empty;
        public string NationalIdNumberImagePath { get; set; } = string.Empty;

        public string ProfileImagePath { get; set; } = string.Empty;

        public StatusEnum Status { get; set; }

        public int AssignedHubZoneId { get; set; }

        [NotMapped]
        public double? Latitude { get; set; }

        [NotMapped]
        public double? Longitude { get; set; }
    }
}


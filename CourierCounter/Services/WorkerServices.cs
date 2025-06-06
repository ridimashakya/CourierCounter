using Azure;
using Azure.Core;
using CourierCounter.Data;
using CourierCounter.Location;
using CourierCounter.Models;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.ApiModels.ApiResponse;
using CourierCounter.Models.Entities;
using CourierCounter.Models.Enum;
using CourierCounter.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Threading.Tasks;

namespace CourierCounter.Services
{
    public class WorkerServices : IWorkerServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INominatimGeocodingService _geoCodingService;

        public WorkerServices(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, INominatimGeocodingService geoCodingService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _geoCodingService = geoCodingService;
        }

        public async Task<ApiResponse<bool>> CreateWorker([FromForm] RegistrationViewModel data)
        {
            ApiResponse<bool> result;

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                ApplicationUser user = new ApplicationUser
                {
                    Email = data.Email,
                    UserName = data.Email
                };

                var res = await _userManager.CreateAsync(user, data.Password);

                if (!res.Succeeded)
                {
                    return new ApiResponse<bool>(false, "Registration Failed!");
                }

                //Prepare folder to save images
                string imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "uploads");
                if (!Directory.Exists(imageFolder))
                {
                    Directory.CreateDirectory(imageFolder);
                }

                string SaveImage(IFormFile file)
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    string filePath = Path.Combine(imageFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Path.Combine("images", "uploads", fileName).Replace("\\", "/");
                }
                var vehicleImagePath = SaveImage(data.VehicleRegistrationNumberImage);
                var licenseImagePath = SaveImage(data.LicenseNumberImage);
                var nidImagePath = SaveImage(data.NationalIdNumberImage);
                var profileImagePath = SaveImage(data.ProfileImage);

                var geoResult = await _geoCodingService.GeocodeAddressAsync(data.HomeAddress);
                if (geoResult == null)
                    throw new Exception("Failed to geocode worker address");

                var workerLat = geoResult.Value.lat;
                var workerLng = geoResult.Value.lng;

                var nearestHubId = HubCoordinates.Locations
            .OrderBy(hub => GeoHelper.GetDistanceInKm(workerLat, workerLng, hub.Value.Lat, hub.Value.Lng))
            .First().Key;

                // Map to database entity including image paths
                Workers workerEntity = new Workers
                {
                    UserId = user.Id,
                    FullName = data.FullName,
                    Email = data.Email,
                    Password = data.Password,
                    ContactNumber = data.ContactNumber,
                    HomeAddress = data.HomeAddress,
                    VehicleRegistrationNumber = data.VehicleRegistrationNumber,
                    LicenseNumber = data.LicenseNumber,
                    NationalIdNumber = data.NationalIdNumber,
                    VehicleRegistrationNumberImagePath = vehicleImagePath,
                    LicenseNumberImagePath = licenseImagePath,
                    NationalIdNumberImagePath = nidImagePath,
                    ProfileImagePath = profileImagePath,
                    Status = StatusEnum.Pending,
                    Latitude = workerLat,
                    Longitude = workerLng,
                    AssignedHubZoneId = nearestHubId
                };

                _dbContext.AllWorkers.Add(workerEntity);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                result = new ApiResponse<bool>(true, "Successfully Registered!");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result = new ApiResponse<bool>(false, "Registration Failed!");
            }

            return result;
        }


        public List<Worker> GetAllWorker(StatusEnum? status = null)
        {
            List<Worker> workers = new List<Worker>();
            try
            {
                if (status == null)
                {
                    workers = (from worker in _dbContext.AllWorkers
                               select new Worker
                               {
                                   Id = worker.Id,
                                   FullName = worker.FullName,
                                   Email = worker.Email,
                                   ContactNumber = worker.ContactNumber,
                                   HomeAddress = worker.HomeAddress,
                                   LicenseNumber = worker.LicenseNumber,
                                   NationalIdNumber = worker.NationalIdNumber,
                                   VehicleRegistrationNumber = worker.VehicleRegistrationNumber,
                                   ProfileImagePath = worker.ProfileImagePath,
                                   Status = worker.Status
                               }).ToList();
                }
                else
                {
                    workers = (from worker in _dbContext.AllWorkers
                               where worker.Status == status
                               select new Worker
                               {
                                   Id = worker.Id,
                                   FullName = worker.FullName,
                                   Email = worker.Email,
                                   ContactNumber = worker.ContactNumber,
                                   HomeAddress = worker.HomeAddress,
                                   LicenseNumber = worker.LicenseNumber,
                                   NationalIdNumber = worker.NationalIdNumber,
                                   VehicleRegistrationNumber = worker.VehicleRegistrationNumber,
                                   ProfileImagePath = worker.ProfileImagePath,
                                   Status = worker.Status
                               }).ToList();
                }

            }
            catch (Exception ex)
            {
                //log exception
            }


            return workers;
        }

        public Worker GetWorkerById(int id)
        {
            Worker? workerValues = new Worker();

            try
            {
                workerValues = (from worker in _dbContext.AllWorkers
                                where worker.Id == id
                                select new Worker
                                {
                                    Id = worker.Id,
                                    FullName = worker.FullName,
                                    Email = worker.Email,
                                    ContactNumber = worker.ContactNumber,
                                    HomeAddress = worker.HomeAddress,
                                    LicenseNumber = worker.LicenseNumber,
                                    NationalIdNumber = worker.NationalIdNumber,
                                    VehicleRegistrationNumber = worker.VehicleRegistrationNumber,
                                    VehicleRegistrationNumberImagePath = worker.VehicleRegistrationNumberImagePath,
                                    LicenseNumberImagePath = worker.LicenseNumberImagePath,
                                    NationalIdNumberImagePath = worker.NationalIdNumberImagePath,
                                    ProfileImagePath = worker.ProfileImagePath,
                                    Status = worker.Status
                                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //log exception
            }

            return workerValues;
        }

        public async Task<WorkerDetailsViewModel> GetWorkerByUserId(string userId)
        {
            var workerDetails = new WorkerDetailsViewModel();

            try
            {
                workerDetails = await (from worker in _dbContext.AllWorkers
                                       where worker.UserId == userId
                                       select new WorkerDetailsViewModel
                                       {
                                           //Id = worker.Id,
                                           FullName = worker.FullName,
                                           Email = worker.Email,
                                           ContactNumber = worker.ContactNumber,
                                           HomeAddress = worker.HomeAddress,
                                           LicenseNumber = worker.LicenseNumber,
                                           NationalIdNumber = worker.NationalIdNumber,
                                           VehicleRegistrationNumber = worker.VehicleRegistrationNumber,
                                           LicenseNumberImagePath = worker.LicenseNumberImagePath,
                                           NationalIdNumberImagePath = worker.NationalIdNumberImagePath,
                                           VehicleRegistrationNumberImagePath = worker.VehicleRegistrationNumberImagePath,
                                           ProfileImagePath = worker.ProfileImagePath
                                           //Status = worker.Status
                                       }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

            }

            return workerDetails;
        }

        public bool UpdateStatusById(int id, StatusEnum status)
        {
            bool result = false;

            try
            {
                var worker = _dbContext.AllWorkers.Find(id);

                if (worker == null)
                    return false;

                worker.Status = status;

                _dbContext.AllWorkers.Update(worker);
                _dbContext.SaveChanges();

                result = true;
            }
            catch (Exception)
            {

            }
            return result;
        }

        public bool DeleteWorker(int id)
        {
            try
            {
                var worker = _dbContext.AllWorkers.Find(id);

                if (worker == null)
                    return false;

                _dbContext.AllWorkers.Remove(worker);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

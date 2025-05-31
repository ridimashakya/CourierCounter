using Azure;
using Azure.Core;
using CourierCounter.Data;
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

        public WorkerServices(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
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
                    Status = StatusEnum.Pending
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

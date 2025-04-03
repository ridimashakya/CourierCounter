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

        public async Task<ApiResponse<bool>> CreateWorker(RegistrationViewModel data)
        {
            ApiResponse<bool> result;

            try
            {
                //add user to identity table
                ApplicationUser user = new ApplicationUser
                {
                    Email = data.Email,
                    UserName = data.Email
                };

                var res = await _userManager.CreateAsync(user, data.Password);

                if (!res.Succeeded)
                    return new ApiResponse<bool>(false, "Registration Failed!");

                else
                {
                    //Mapping for database entity
                    Workers workerEntity = new Workers
                    {
                        FullName = data.FullName,
                        Email = data.Email,
                        Password = data.Password,
                        ContactNumber = data.ContactNumber,
                        HomeAddress = data.HomeAddress,
                        VehicleRegistrationNumber = data.VehicleRegistrationNumber,
                        LicenseNumber = data.LicenseNumber,
                        NationalIdNumber = data.NationalIdNumber,
                        Status = Models.Enum.StatusEnum.Pending
                    };

                    _dbContext.AllWorkers.Add(workerEntity);
                    _dbContext.SaveChanges();

                    result = new ApiResponse<bool>(true, "Successfully Registered!");
                }
            }
            catch (Exception ex)
            {
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
                                    Status = worker.Status
                                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //log exception
            }

            return workerValues;
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

        public ApiResponse<bool> UpdateWorker(RegistrationViewModel data)
        {
            ApiResponse<bool> result;
            try
            {
                // update to database logic

                //Mapping for database entry
                Workers workerEntity = new Workers
                {
                    FullName = data.FullName,
                    Email = data.Email,
                    Password = data.Password,
                    ContactNumber = data.ContactNumber,
                    HomeAddress = data.HomeAddress,
                    VehicleRegistrationNumber = data.VehicleRegistrationNumber,
                    LicenseNumber = data.LicenseNumber,
                    NationalIdNumber = data.NationalIdNumber
                };

                result = new ApiResponse<bool>(true, "Updated Successfully!");
            }
            catch (Exception ex)
            {
                result = new ApiResponse<bool>(false, "Update Failed!");
            }
            return result;
        }
    }
}

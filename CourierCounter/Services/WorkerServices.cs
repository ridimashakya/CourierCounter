using CourierCounter.Data;
using CourierCounter.Models;
using CourierCounter.Models.ApiModels;
using CourierCounter.Models.ApiModels.ApiResponse;
using CourierCounter.Models.Entities;
using CourierCounter.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
                        NationalIdNumber = data.NationalIdNumber
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

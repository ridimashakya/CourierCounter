using CourierCounter.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CourierCounter.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //property
        //the Workers here is the entity we create for workers

        public DbSet<Workers> AllWorkers { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<WorkerOrders> WorkerOrder { get; set; }
    }
}

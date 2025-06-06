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
        public DbSet<DailyEarning> DailyEarnings { get; set; }
        public DbSet<EarningHistory> EarningHistories { get; set; }
        public DbSet<WageTrainingData> WageTrainingDataset { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Workers>().ToTable("AllWorkers");

            modelBuilder.Entity<WorkerOrders>()
                .HasOne<Orders>()
                .WithMany()
                .HasForeignKey(wo => wo.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkerOrders>()
                .HasOne<Workers>()
                .WithMany()
                .HasForeignKey(wo => wo.WorkerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

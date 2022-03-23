using CleaningManagement.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CleaningManagement.DAL
{
    public class CleaningManagementDbContext : DbContext
    {
        public string DbPath { get; }
        public DbSet<CleaningPlan> CleaningPlans { get; set; }

        public CleaningManagementDbContext()
        {
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseInMemoryDatabase("CleaningContext");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CleaningPlan>()
                .Property(obj => obj.Description)
                .IsRequired(false)
                .HasMaxLength(512);

            modelBuilder.Entity<CleaningPlan>()
                .Property(obj => obj.Title)
                .HasMaxLength(256);

            base.OnModelCreating(modelBuilder);

        }
    }
}
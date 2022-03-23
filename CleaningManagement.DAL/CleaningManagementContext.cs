using Microsoft.EntityFrameworkCore;

namespace CleaningManagement.DAL
{
    public class CleaningManagementDbContext : DbContext
    {
        public string DbPath { get; }

        public CleaningManagementDbContext()
        {
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseInMemoryDatabase("CleaningContext");
    }
}
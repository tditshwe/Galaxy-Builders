using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using GalaxyBuildersSystem.Models;

namespace GalaxyBuildersSystem
{
    public class GalaxyContext : DbContext
    {
        public GalaxyContext() : base("DefaultConnection")
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<GTask> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}

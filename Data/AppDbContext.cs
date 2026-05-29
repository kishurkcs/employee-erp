using Microsoft.EntityFrameworkCore;
using EmployeeApi.Models;

namespace EmployeeApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<EmployeeSalary> EmployeeSalaries => Set<EmployeeSalary>();

        public DbSet<EmployeeLeave> EmployeeLeaves => Set<EmployeeLeave>();

        public DbSet<EmployeeProfile> EmployeeProfiles => Set<EmployeeProfile>();

        public DbSet<AppUser> Users => Set<AppUser>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmployeeSalary>()
                .Property(s => s.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
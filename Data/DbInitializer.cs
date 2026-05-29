using EmployeeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAdminUserAsync(AppDbContext context)
        {
            if (await context.Users.AnyAsync())
            {
                return;
            }

            context.Users.Add(new AppUser
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123")
            });

            await context.SaveChangesAsync();
        }
    }
}

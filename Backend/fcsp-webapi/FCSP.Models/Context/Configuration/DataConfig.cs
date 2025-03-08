using FCSP.Common.Enums;
using FCSP.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCSP.Models.Context.Configuration;

internal static class DataConfig
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Name = "Admin",
                Email = "admin@gmail.com",
                PasswordHash = "Admin@123",
                UserRole = UserRole.Admin,
                Status = 1,
                IsDeleted = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
    }
}

using LMS.Api.Models;

namespace LMS.Api.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        // Ensure database exists
        context.Database.EnsureCreated();

        // Seed Roles ONLY if empty
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { RoleName = "Student" },
                new Role { RoleName = "Teacher" },
                new Role { RoleName = "Admin" }
            );

            context.SaveChanges();
        }
    }
}

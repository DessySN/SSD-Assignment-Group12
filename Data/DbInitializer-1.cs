
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using SSD_LAB1.Models;

namespace SSD_LAB1.Data
{
    public static class DbInitializer
    {
        public static async Task<int> SeedUsersAndRoles(
            IServiceProvider serviceProvider)
        {
            // Create the database if it doesn't exist
            var context =
                serviceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();

            var configuration =
                serviceProvider.GetRequiredService<IConfiguration>();

            var seedPassword = configuration["SeedUsers:Password"];

            if (string.IsNullOrWhiteSpace(seedPassword))
            {
                throw new InvalidOperationException(
                    "The seeded-user password has not been configured.");
            }

            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Check if roles already exist and exit if there are
            if (roleManager.Roles.Any())
                return 1;

            // Seed roles
            int result = await SeedRoles(roleManager);

            if (result != 0)
                return 2;

            // Check if users already exist and exit if there are
            if (userManager.Users.Any())
                return 3;

            // Seed users
            result = await SeedUsers(userManager, seedPassword);

            if (result != 0)
                return 4;

            return 0;
        }

        private static async Task<int> SeedRoles(
            RoleManager<IdentityRole> roleManager)
        {
            // Create Supervisor role
            var result = await roleManager.CreateAsync(
                new IdentityRole("Supervisor"));

            if (!result.Succeeded)
                return 1;

            // Create Employee role
            result = await roleManager.CreateAsync(
                new IdentityRole("Employee"));

            if (!result.Succeeded)
                return 2;

            return 0;
        }

        private static async Task<int> SeedUsers(
            UserManager<ApplicationUser> userManager,
            string seedPassword)
        {
            // Create Supervisor user
            var supervisorUser = new ApplicationUser
            {
                UserName = "supervisor@mohawkcollege.ca",
                Email = "supervisor@mohawkcollege.ca",
                FirstName = "The",
                LastName = "Supervisor",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(
                supervisorUser, seedPassword);

            if (!result.Succeeded)
                return 1;

            // Assign Supervisor role
            result = await userManager.AddToRoleAsync(
                supervisorUser, "Supervisor");

            if (!result.Succeeded)
                return 2;

            // Create Employee user
            var employeeUser = new ApplicationUser
            {
                UserName = "employee@mohawkcollege.ca",
                Email = "employee@mohawkcollege.ca",
                FirstName = "The",
                LastName = "Employee",
                EmailConfirmed = true
            };

            result = await userManager.CreateAsync(
                employeeUser, seedPassword);

            if (!result.Succeeded)
                return 3;

            // Assign Employee role
            result = await userManager.AddToRoleAsync(
                employeeUser, "Employee");

            if (!result.Succeeded)
                return 4;

            return 0;
        }
    }
}

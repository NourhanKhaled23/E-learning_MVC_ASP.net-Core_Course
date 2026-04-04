using Microsoft.AspNetCore.Identity;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1
{
    public static class SeedData
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "Student", "Instructor" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminUser = await userManager.FindByEmailAsync("admin@example.com");
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FullName = "Site Administrator"
                };

                var createPowerUser = await userManager.CreateAsync(newAdmin, "Admin123!");
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }

        public static void Initialize(AppDbContext context)
        {
            if (context.Students.Any() || context.Departments.Any())
            {
                return;
            }

            var departments = new Department[]
            {
                new()
                {
                    Name = "Computer Science",
                    Location = "Building A, Floor 3",
                    PhoneNumber = "123-456-7890"
                },
                new()
                {
                    Name = "Mathematics",
                    Location = "Building B, Floor 2",
                    PhoneNumber = "123-456-7891"
                },
                new()
                {
                    Name = "Physics",
                    Location = "Building C, Floor 1",
                    PhoneNumber = "123-456-7892"
                }
            };

            context.Departments.AddRange(departments);
            context.SaveChanges();

            var csDept = context.Departments.First(d => d.Name == "Computer Science");
            var mathDept = context.Departments.First(d => d.Name == "Mathematics");
            var physicsDept = context.Departments.First(d => d.Name == "Physics");

            context.Students.AddRange(
                new Student
                {
                    Name = "Alice Johnson",
                    Age = 20,
                    Address = "123 Main St",
                    Email = "alice@example.com",
                    Image = "placeholder.svg",
                    DeptId = csDept.DeptId
                },
                new Student
                {
                    Name = "Bob Smith",
                    Age = 22,
                    Address = "456 Oak Ave",
                    Email = "bob@example.com",
                    Image = "placeholder.svg",
                    DeptId = mathDept.DeptId
                },
                new Student
                {
                    Name = "Carol White",
                    Age = 21,
                    Address = "789 Pine Rd",
                    Email = "carol@example.com",
                    Image = "placeholder.svg",
                    DeptId = csDept.DeptId
                },
                new Student
                {
                    Name = "David Brown",
                    Age = 23,
                    Address = "321 Elm St",
                    Email = "david@example.com",
                    Image = "placeholder.svg",
                    DeptId = physicsDept.DeptId
                },
                new Student
                {
                    Name = "Eva Green",
                    Age = 20,
                    Address = "654 Maple Dr",
                    Email = "eva@example.com",
                    Image = "placeholder.svg",
                    DeptId = mathDept.DeptId
                }
            );

            context.SaveChanges();
        }
    }
}
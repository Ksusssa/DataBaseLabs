
using lab6.WebApplication.Contexts;
using Lab6.WebApplication.Models;
using Lab6.WebApplication.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace InsuranceCompany.Data.Internal.Helpers
{
    public static class DbInitializerMiddlewares
    {
        private static readonly List<string> roles = new()
        {
            "Admin",
            "Employee",
            "Client"
        };

        public static void InitializeData(InsuranceCompanyDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            var isDataInserted = dbContext.Risks.Any()
                || dbContext.TypeOfPolicies.Any()
                || dbContext.Policies.Any();

            if (isDataInserted)
            {
                return;
            }

            var randomObject = new Random();
            var rowCount = 20;

            for (int i = 0; i < rowCount; i++)
            {
                var risk = new Risk
                {
                    AverageProbability = randomObject.Next(1, 10),
                    Name = "Risk #" + i,
                    Description = "Description of the Risk #" + i,
                };

                var policyType = new PolicyType
                {
                    Name = "Policy Type #" + i,
                    Condition = "Condition of the type #" + i,
                    Description = "Description of the typ #" + i
                };

                dbContext.Risks.Add(risk);
                dbContext.TypeOfPolicies.Add(policyType);
            }

            dbContext.SaveChanges();

            for (int i = 0; i < rowCount; i++)
            {
                var clientId = randomObject.Next(1, rowCount);
                var employeeId = randomObject.Next(1, rowCount);

                var policy = new Policy
                {
                    StartDate = DateTime.Now,
                    FinishDate = DateTime.Now.AddDays(randomObject.Next(1, 100)),
                    Price = randomObject.Next(1, 100),
                    Payment = randomObject.Next(1, 100),
                    PolicyTypeId = randomObject.Next(1, rowCount),
                    ClientId = clientId,
                    EmployeeId = employeeId,
                };

                if (employeeId > 13) policy.ClientId = null;

                dbContext.Policies.Add(policy);
            }

            dbContext.SaveChanges();
        }

        public static async Task InitializeRoles(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@mail.ru";
            string adminPassword = "Admin123!";

            foreach (var role in roles)
            {
                if (await roleManager.FindByNameAsync(role) is null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            if (await userManager.FindByEmailAsync(adminEmail) is null)
            {
                var admin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    PhoneNumber = "+375441111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                };

                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }

        public static async Task InitializeUsers(
            InsuranceCompanyDbContext dbContext,
            UserManager<User> userManager)
        {
            var isDataInserted = dbContext.Clients.Any()
                || dbContext.Employees.Any();

            if (isDataInserted)
            {
                return;
            }

            var rowCount = 20;
            var clientDefaultPassword = "Client123!";
            var emplDefaultPassword = "Empl123!";

            for (int i = 0; i < rowCount; i++)
            {
                var user = new User
                {
                    UserName = $"client-{i}@mail.com",
                    Email = $"client-{i}@mail.com",
                    PhoneNumber = $"+3754{i}111{i}111",
                };

                IdentityResult result = await userManager.CreateAsync(user, clientDefaultPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Client");
                }

                var client = new Client
                {
                    User = user,
                    Address = "Empty",
                    BornDate = DateTime.Now,
                    Passport = "Nope",
                    Phone = $"+3754{i}111{i}111",
                };

                await dbContext.Clients.AddAsync(client);
            }

            for (int i = 0; i < rowCount; i++)
            {
                var user = new User
                {
                    UserName = $"empl{i}@mail.com",
                    Email = $"empl{i}@mail.com",
                    PhoneNumber = $"+3754{i}111{i}111",
                };

                IdentityResult result = await userManager.CreateAsync(user, emplDefaultPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Employee");
                }

                var employese= new Employee
                {
                    User = user,
                    TypeOfPost = PostType.SalesAgent,
                    Experience = i,
                };

                await dbContext.Employees.AddAsync(employese);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}

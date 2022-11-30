using InsuranceCompany.Common.Enums.Data;
using InsuranceCompany.Data.Internal.Entities;
using InsuranceCompany.Data.Internal.Contexts;
using Microsoft.AspNetCore.Identity;

namespace InsuranceCompany.Data.Internal.Helpers
{
    public static class DbInitializer
    {
        private static readonly List<string> roles = new()
        {
            "Admin",
            "Employee"
        };

        public static void Initialize(InsuranceCompanyDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            var isDataInserted = dbContext.Risks.Any()
                || dbContext.TypeOfPolicies.Any()
                || dbContext.Clients.Any()
                || dbContext.Employees.Any()
                || dbContext.Policies.Any();

            if (isDataInserted)
            {
                return;
            }

            var randomObject = new Random();
            var rowCount = 10;

            var namesGenderTuple = new List<Tuple<string, Gender>>
            {
                new Tuple<string, Gender>("Dmitry", Gender.Male),
                new Tuple<string, Gender>("Ivan", Gender.Male),
                new Tuple<string, Gender>("Oleg", Gender.Male),
                new Tuple<string, Gender>("Roman", Gender.Male),
                new Tuple<string, Gender>("Pavel", Gender.Male),
                new Tuple<string, Gender>("Maxim", Gender.Male),
                new Tuple<string, Gender>("Evgeniy", Gender.Male),
                new Tuple<string, Gender>("Vyeacheslav", Gender.Male),
                new Tuple<string, Gender>("Vitaliy", Gender.Male),
                new Tuple<string, Gender>("Bogdan", Gender.Male),
                new Tuple<string, Gender>("Zahar", Gender.Male),
                new Tuple<string, Gender>("Anton", Gender.Male),
                new Tuple<string, Gender>("Olga", Gender.Female),
                new Tuple<string, Gender>("Anastasiya", Gender.Female),
                new Tuple<string, Gender>("Viktoria", Gender.Female),
                new Tuple<string, Gender>("Elena", Gender.Female),
                new Tuple<string, Gender>("Vera", Gender.Female),
                new Tuple<string, Gender>("Kristina", Gender.Female),
                new Tuple<string, Gender>("Veronica", Gender.Female),
                new Tuple<string, Gender>("Margarita", Gender.Female),
            };

            var lastNames = new string[]
            {
                "Karavay",
                "Fedor",
                "Borisenko",
                "Tutovich",
                "Vidovich",
                "Isai",
                "Minenko",
                "Aloha",
                "Avocs",
                "Tyet",
                "Zayevei",
                "Aserovich",
                "Himik",
                "Volodko",
                "Malinoski",
                "Hozevich",
                "Kone",
                "Tritovich",
                "Veros",
                "Hev",
                "Kev",
                "Old",
                "Mitronivich",
                "Barbos"
            };

            var middleNames = new string[] { "Viktorov.", "Olegov.", "Vladimirov.", "Fedorov.", "Evgeniev." };

            for (int i = 0; i < rowCount; i++)
            {
                var nameGenderInfoClient = namesGenderTuple[randomObject.Next(namesGenderTuple.Count)];
                var client = new Client
                {
                    FirstName = nameGenderInfoClient.Item1,
                    LastName = lastNames[randomObject.Next(lastNames.Length)],
                    MiddleName = middleNames[randomObject.Next(middleNames.Length)],
                    Address = "Main Street House #" + i,
                    Gender = nameGenderInfoClient.Item2,
                    BornDate = DateTime.Now,
                    Phone = "+3123" + randomObject.Next().ToString() + i,
                    Passport = "NOT ADDED"
                };

                var nameGenderInfoEmployee = namesGenderTuple[randomObject.Next(namesGenderTuple.Count)];
                var employee = new Employee
                {
                    FirstName = nameGenderInfoEmployee.Item1,
                    LastName = lastNames[randomObject.Next(lastNames.Length)],
                    MiddleName = middleNames[randomObject.Next(middleNames.Length)],
                    Gender = nameGenderInfoEmployee.Item2,
                    TypeOfPost = PostType.SalesAgent,
                    Experience = randomObject.Next(1, 10),
                };

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
                dbContext.Clients.Add(client);
                dbContext.Employees.Add(employee);
            }

            dbContext.SaveChanges();

            var policiesRowCount = rowCount * 2;
            for (int i = 0; i < policiesRowCount; i++)
            {
                var clientId = randomObject.Next(1, rowCount);
                var employeeId = randomObject.Next(1, rowCount);

                var policy = new Policy
                {
                    StartDate = DateTime.Now,
                    FinishDate = DateTime.Now.AddDays(randomObject.Next(1, 100)),
                    Price = 500 * (float)randomObject.NextDouble(),
                    Payment = 100 * (float)randomObject.NextDouble(),
                    PolicyTypeId = randomObject.Next(1, rowCount),
                    ClientId = clientId,
                    EmployeeId = employeeId,
                };

                dbContext.Policies.Add(policy);
            }

            dbContext.SaveChanges();
        }

        public static async Task InitializeRoles(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@mail.ru";
            string adminPassword = "Admin";

            foreach (var role in roles)
            {
                if (await roleManager.FindByNameAsync(role) is null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            if (await userManager.FindByNameAsync(adminEmail) is null)
            {
                var admin = new IdentityUser
                {
                    UserName = "Admin",
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
    }
}

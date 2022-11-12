using InsuranceCompany.Data.Contracts.Entities;
using InsuranceCompany.Data.Internal.Contexts;

namespace InsuranceCompany.Data.Internal.Helpers
{
    public static class DbInitializer
    {
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

            for(int i = 0; i < 5; i++)
            {
                var client = new Client
                {
                    FirstName = "Client" + i,
                    LastName = "L Client" + i,
                    MiddleName = "M Client" + i,
                    Address = "Test adres #" + i,
                    Gender = Common.Enums.Data.Gender.Male,
                    BornDate = DateTime.Now,
                    Phone = "+1231313" + i,
                    Passport = "not exists"
                };

                var employee = new Employee
                {
                    FirstName = "Empl" + i,
                    LastName = "L Empl" + i,
                    MiddleName = "M Empl" + i,
                    Gender = Common.Enums.Data.Gender.Male,
                    TypeOfPost = Common.Enums.Data.PostType.SalesAgent,
                    Experience = 1,
                };

                var risk = new Risk
                {
                    Name = "Risk" + i,
                    Description = "Desc" + i,
                    AverageProbability = i
                };

                var policyType = new PolicyType
                {
                    Name = "Ptype " + i,
                    Condition = "Cond" + i,
                    Description = "Desc" + i
                };

                dbContext.Clients.Add(client);
                dbContext.Employees.Add(employee);
                dbContext.Risks.Add(risk);
                dbContext.TypeOfPolicies.Add(policyType);

                dbContext.SaveChanges();

                var policy = new Policy
                {
                    StartDate = DateTime.Now,
                    FinishDate = DateTime.Now.AddDays(1),
                    Price = i * 200,
                    Payment = i,
                    PolicyType = policyType,
                    Client = client,
                    Employee = employee
                };

                dbContext.Policies.Add(policy);
                dbContext.SaveChanges();
            }
        }
    }
}

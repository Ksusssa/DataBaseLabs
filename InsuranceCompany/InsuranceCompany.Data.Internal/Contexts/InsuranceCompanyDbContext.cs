using InsuranceCompany.Data.Contracts.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCompany.Data.Internal.Contexts
{
    public class InsuranceCompanyDbContext : IdentityDbContext
    {
        public InsuranceCompanyDbContext(DbContextOptions<InsuranceCompanyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Policy> Policies { get; set; }
        
        public DbSet<Risk> Risks { get; set; }
        
        public DbSet<PolicyType> TypeOfPolicies { get; set; }
    }
}
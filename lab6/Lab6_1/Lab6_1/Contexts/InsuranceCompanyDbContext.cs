
using Lab6.WebApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace lab6.WebApplication.Contexts
{
    public class InsuranceCompanyDbContext : IdentityDbContext<User>
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
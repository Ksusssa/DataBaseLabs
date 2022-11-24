using InsuranceCompany.Data.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceCompany.Data.Internal.EntityConfigurations
{
    public class PolicyEntityConfiguration : IEntityTypeConfiguration<Policy>
    {
        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            builder.HasOne(p => p.Client)
                .WithMany(c => c.Policies)
                .HasForeignKey(p => p.ClientId);

            builder.HasOne(p => p.Employee)
                .WithMany(e => e.Policies)
                .HasForeignKey(p => p.EmployeeId);

            builder.HasOne(p => p.PolicyType)
                .WithMany()
                .HasForeignKey(p => p.PolicyTypeId);
        }
    }
}
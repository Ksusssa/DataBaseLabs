using InsuranceCompany.Data.Internal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsuranceCompany.Data.Internal.EntityConfigurations
{
    public class PolicyTypeEntityConfiguration : IEntityTypeConfiguration<PolicyType>
    {
        public void Configure(EntityTypeBuilder<PolicyType> builder)
        {
            builder.HasMany(pT => pT.Risks)
                .WithMany(r => r.TypesOfPolicies);
        }
    }
}
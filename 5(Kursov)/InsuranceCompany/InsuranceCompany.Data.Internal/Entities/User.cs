using InsuranceCompany.Common.Enums.Data;
using Microsoft.AspNetCore.Identity;

namespace InsuranceCompany.Data.Internal.Entities
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? MiddleName { get; set; }

        public GenderType? Gender { get; set; }
    }
}

using Lab6.WebApplication.Models.Enums;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lab6.WebApplication.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? MiddleName { get; set; }

        public GenderType? Gender { get; set; }
    }
}

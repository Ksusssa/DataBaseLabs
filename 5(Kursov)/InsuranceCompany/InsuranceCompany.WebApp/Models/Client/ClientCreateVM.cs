using InsuranceCompany.Common.Enums.Data;

namespace InsuranceCompany.WebApp.Models.Client
{
    public class ClientCreateVM
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public DateTime BornDate { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Passport { get; set; }

        public GenderType Gender { get; set; }
    }
}

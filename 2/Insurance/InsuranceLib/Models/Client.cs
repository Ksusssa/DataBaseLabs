using System;
using System.Collections.Generic;

namespace InsuranceLib.Models
{
    public partial class Client
    {
        public Client()
        {
            Policies = new HashSet<Policy>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public DateTime BornDate { get; set; }
        public string? Gender { get; set; }
        public string? Adres { get; set; }
        public int Phone { get; set; }
        public string Passport { get; set; } = null!;

        public virtual ICollection<Policy> Policies { get; set; }
    }
}

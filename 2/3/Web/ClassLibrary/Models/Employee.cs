using System;
using System.Collections.Generic;

namespace InsuranceLib.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Policies = new HashSet<Policy>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string Post { get; set; } = null!;
        public int Experience { get; set; }

        public virtual ICollection<Policy> Policies { get; set; }
    }
}

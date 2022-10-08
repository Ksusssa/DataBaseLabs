using System;
using System.Collections.Generic;

namespace InsuranceLib.Models
{
    public partial class Policy
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public double Price { get; set; }
        public double? Payment { get; set; }
        public int TypesOfPolicieId { get; set; }
        public int? IsPaied { get; set; }
        public int? IsFinish { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;
        public virtual TypesOfPolicy TypesOfPolicie { get; set; } = null!;
    }
}

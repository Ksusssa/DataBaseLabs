using System;
using System.Collections.Generic;

namespace InsuranceLib.Models
{
    public partial class TypesOfPoliciesAndRisk
    {
        public int Id { get; set; }
        public int TypesOfPolicieId { get; set; }
        public int RiskId { get; set; }

        public virtual Risk Risk { get; set; } = null!;
        public virtual TypesOfPolicy TypesOfPolicie { get; set; } = null!;
    }
}

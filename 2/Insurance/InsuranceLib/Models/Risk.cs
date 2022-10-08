using System;
using System.Collections.Generic;

namespace InsuranceLib.Models
{
    public partial class Risk
    {
        public Risk()
        {
            TypesOfPoliciesAndRisks = new HashSet<TypesOfPoliciesAndRisk>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double AverageProbability { get; set; }

        public virtual ICollection<TypesOfPoliciesAndRisk> TypesOfPoliciesAndRisks { get; set; }
    }
}

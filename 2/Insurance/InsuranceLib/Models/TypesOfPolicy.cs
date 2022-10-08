using System;
using System.Collections.Generic;

namespace InsuranceLib.Models
{
    public partial class TypesOfPolicy
    {
        public TypesOfPolicy()
        {
            Policies = new HashSet<Policy>();
            TypesOfPoliciesAndRisks = new HashSet<TypesOfPoliciesAndRisk>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Condition { get; set; }

        public virtual ICollection<Policy> Policies { get; set; }
        public virtual ICollection<TypesOfPoliciesAndRisk> TypesOfPoliciesAndRisks { get; set; }
    }
}

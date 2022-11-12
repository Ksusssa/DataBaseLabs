using System.Collections.Generic;
using InsuranceCompany.Data.Contracts.Entities.Base;

namespace InsuranceCompany.Data.Contracts.Entities
{
    public class Risk : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }
        
        public float AverageProbability { get; set; }
        
        public ICollection<PolicyType> TypesOfPolicies { get; set; } = new List<PolicyType>();
    }
}
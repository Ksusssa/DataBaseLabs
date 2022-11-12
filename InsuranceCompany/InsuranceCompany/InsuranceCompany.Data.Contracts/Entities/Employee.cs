using System.Collections.Generic;
using InsuranceCompany.Common.Enums.Data;
using InsuranceCompany.Data.Contracts.Entities.Base;

namespace InsuranceCompany.Data.Contracts.Entities
{
    public class Employee : HumanBase
    {
        public PostType TypeOfPost { get; set; }
        
        public long Experience { get; set; }
        
        public ICollection<Policy> Policies { get; set; } = new List<Policy>();
    }
}
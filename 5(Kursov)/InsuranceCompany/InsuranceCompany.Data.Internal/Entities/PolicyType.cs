using InsuranceCompany.Data.Internal.Entities.Base;

namespace InsuranceCompany.Data.Internal.Entities
{
    public class PolicyType : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }
        
        public string Condition { get; set; }
        
        public ICollection<Risk> Risks { get; set; } = new List<Risk>();
    }
}
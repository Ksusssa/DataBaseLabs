
using Lab6.WebApplication.Models.Base;

namespace Lab6.WebApplication.Models
{
    public class Risk : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }
        
        public float AverageProbability { get; set; }
        
        public ICollection<PolicyType> TypesOfPolicies { get; set; } = new List<PolicyType>();
    }
}
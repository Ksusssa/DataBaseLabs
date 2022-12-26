
using Lab6.WebApplication.Models.Base;

namespace Lab6.WebApplication.Models
{
    public class PolicyType : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }
        
        public string Condition { get; set; }
        
        public ICollection<Risk> Risks { get; set; } = new List<Risk>();
    }
}
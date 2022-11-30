using InsuranceCompany.Data.Internal.Entities.Base;

namespace InsuranceCompany.Data.Internal.Entities
{
    public class Client : HumanBase
    {
        public DateTime BornDate { get; set; }
        
        public string Address { get; set; }
        
        public string Phone { get; set; }
        
        public string Passport { get; set; }
        
        public ICollection<Policy> Policies { get; set; } = new List<Policy>();
    }
}
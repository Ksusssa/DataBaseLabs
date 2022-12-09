using InsuranceCompany.Common.Enums.Data;
using InsuranceCompany.Data.Internal.Entities.Base;

namespace InsuranceCompany.Data.Internal.Entities
{
    public class Employee : EntityBase
    {
        public PostType TypeOfPost { get; set; }
        
        public long Experience { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

        public ICollection<Policy> Policies { get; set; } = new List<Policy>();
    }
}
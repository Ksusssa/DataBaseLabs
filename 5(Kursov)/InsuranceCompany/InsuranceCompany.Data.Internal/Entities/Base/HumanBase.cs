using InsuranceCompany.Common.Enums.Data;

namespace InsuranceCompany.Data.Internal.Entities.Base
{
    public class HumanBase : EntityBase
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public Gender Gender { get; set; }
    }
}

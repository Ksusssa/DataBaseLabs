using Lab6.WebApplication.Models.Enums;

namespace Lab6.WebApplication.Models.Base
{

    public class HumanBase : EntityBase
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public GenderType Gender { get; set; }
    }
}

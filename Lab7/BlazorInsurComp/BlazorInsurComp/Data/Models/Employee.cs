using Lab6.WebApplication.Models;
using Lab6.WebApplication.Models.Base;
using Lab6.WebApplication.Models.Enums;

namespace Lab6.WebApplication.Models
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
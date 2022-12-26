
using Lab6.WebApplication.Models.Base;

namespace Lab6.WebApplication.Models
{
    public class Client : EntityBase
    {
        public DateTime BornDate { get; set; }
        
        public string Address { get; set; }
        
        public string Phone { get; set; }
        
        public string Passport { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }
        
        public ICollection<Policy> Policies { get; set; } = new List<Policy>();
    }
}
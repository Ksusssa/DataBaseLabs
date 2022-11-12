using InsuranceCompany.Data.Contracts.Entities.Base;

namespace InsuranceCompany.Data.Contracts.Entities
{
    public class Policy : EntityBase
    {
        public DateTime StartDate { get; set; }
        
        public DateTime FinishDate { get; set; }
        
        public float Price { get; set; }
        
        public float Payment { get; set; }
        
        public bool IsPaied { get; set; }
        
        public bool IsFinish { get; set; }
        
        public long PolicyTypeId { get; set; }
        
        public PolicyType PolicyType { get; set; }
        
        public long ClientId { get; set; }
        
        public Client Client { get; set; }
        
        public long EmployeeId { get; set; }
        
        public Employee Employee { get; set; }
    }
}
namespace InsuranceCompany.WebApp.Models.Policy
{
    public class PolicyVM
    {
        public long Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public float Price { get; set; }

        public float Payment { get; set; }

        public long PolicyTypeId { get; set; }

        public long EmployeeId { get; set; }
    }
}

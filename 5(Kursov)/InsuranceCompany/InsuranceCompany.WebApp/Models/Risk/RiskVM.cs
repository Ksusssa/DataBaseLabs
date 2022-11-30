namespace InsuranceCompany.WebApp.Models.Risk
{
    public class RiskVM
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float AverageProbability { get; set; }
    }
}

using InsuranceCompany.WebApp.Models.Risk;

namespace InsuranceCompany.WebApp.Models.PolicyType
{
    public class PolicyTypeVM : PolicyTypeCreateVM
    {
        public ICollection<RiskVM> Risks { get; set; } = new List<RiskVM>();
    }
}

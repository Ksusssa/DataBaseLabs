using InsuranceCompany.WebApp.Models.Risk;

namespace InsuranceCompany.WebApp.Models.PolicyType
{
    public class PolicyTypeUpdateRiskVM
    {
        public long RiskId { get; set; }
         
        public string RiskName { get; set; }

        public bool IsSelected { get; set; }
    }
}
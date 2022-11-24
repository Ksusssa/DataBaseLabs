using InsuranceCompany.Data.Internal.Contexts;
using InsuranceCompany.WebApp.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCompany.WebApp.Controllers
{
    public class RiskController : BaseController
    {
        public RiskController(InsuranceCompanyDbContext dbContext) : base(dbContext)
        {
        }

        [ResponseCache(CacheProfileName = "LabaCacheProfile")]
        public async Task<IActionResult> Index()
        {
            var risks = await dbContext.Risks
                .Include(r => r.TypesOfPolicies)
                .ToListAsync();

            return View(risks);
        }
    }
}

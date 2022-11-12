using InsuranceCompany.Data.Internal.Contexts;
using InsuranceCompany.WebApp.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCompany.WebApp.Controllers
{
    public class PolicyController : BaseController
    {
        public PolicyController(InsuranceCompanyDbContext dbContext) : base(dbContext)
        {
        }

        [ResponseCache(CacheProfileName = "LabaCacheProfile")]
        public async Task<IActionResult> Index()
        {
            var policies = await dbContext.Policies
                .Include(p => p.Employee)
                .Include(p => p.Client)
                .Include(p => p.PolicyType)
                .ToListAsync();

            return View(policies);
        }
    }
}

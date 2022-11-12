using InsuranceCompany.Data.Internal.Contexts;
using InsuranceCompany.WebApp.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCompany.WebApp.Controllers
{
    public class ClientController : BaseController
    {
        public ClientController(InsuranceCompanyDbContext dbContext) : base(dbContext)
        {
        }

        [ResponseCache(CacheProfileName = "LabaCacheProfile")]
        public async Task<ActionResult> Index()
        {
            var clients = await dbContext.Clients
                .Include(c => c.Policies)
                .ThenInclude(p => p.PolicyType)
                .ToListAsync();

            return View(clients);
        }
    }
}

using InsuranceCompany.Data.Internal.Entities;
using InsuranceCompany.Data.Internal.Contexts;
using InsuranceCompany.WebApp.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using InsuranceCompany.WebApp.Models.Risk;

namespace InsuranceCompany.WebApp.Controllers
{
    //[Authorize(Roles = "Employee")]
    public class RisksController : BaseController
    {
        public RisksController(InsuranceCompanyDbContext dbContext) : base(dbContext)
        {
        }

        [ResponseCache(CacheProfileName = "LabaCacheProfile")]
        public async Task<IActionResult> Index()
        {
            var risks = await dbContext.Risks
                .Include(r => r.TypesOfPolicies)
                .AsNoTracking()
                .Select(r => 
                    new RiskVM {
                        Id = r.Id,
                        Name = r.Name,
                        Description = r.Description,
                        AverageProbability = r.AverageProbability 
                    })
                .ToListAsync();

            return View(risks);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RiskVM riskCreateVM)
        {
            var risk = new Risk
            {
                Name = riskCreateVM.Name,
                Description = riskCreateVM.Description,
                AverageProbability = riskCreateVM.AverageProbability,
            };

            await dbContext.Risks.AddAsync(risk);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(long id)
        {
            var risk = await dbContext.Risks.FindAsync(id);

            if (risk == null)
            {
                return NotFound();
            }

            var riskVM = new RiskVM
            {
                Id = id,
                Name = risk.Name,
                Description = risk.Description,
                AverageProbability = risk.AverageProbability,
            };

            return View(riskVM);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RiskVM riskVM)
        {
            var risk = await dbContext.Risks.FindAsync(riskVM.Id);

            if (risk == null)
            {
                return NotFound();
            }

            risk.Name = riskVM.Name;
            risk.Description = riskVM.Description;
            risk.AverageProbability = riskVM.AverageProbability;

            dbContext.Risks.Update(risk);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(long id)
        {
            var risk = await dbContext.Risks.FindAsync(id);

            if (risk == null)
            {
                return NotFound();
            }

            dbContext.Risks.Remove(risk);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}

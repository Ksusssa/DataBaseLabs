using InsuranceCompany.Data.Internal.Contexts;
using InsuranceCompany.Data.Internal.Entities;
using InsuranceCompany.WebApp.Controllers.Base;
using InsuranceCompany.WebApp.Models.PolicyType;
using InsuranceCompany.WebApp.Models.Risk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCompany.WebApp.Controllers
{
    //[Authorize(Roles = "Employee")]
    public class PolicyTypesController : BaseController
    {
        public PolicyTypesController(InsuranceCompanyDbContext dbContext) : base(dbContext)
        {
        }

        [ResponseCache(CacheProfileName = "LabaCacheProfile")]
        public async Task<IActionResult> Index()
        {
            var policyTypes = await dbContext.TypeOfPolicies
                .Include(r => r.Risks)
                .AsNoTracking()
                .ToListAsync();

            var policyTypesVM = policyTypes.Select(p => new PolicyTypeVM
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Condition = p.Condition,
                Risks = p.Risks.Select(r => new RiskVM
                { 
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    AverageProbability = r.AverageProbability,
                })
                .ToList(),
            });

            return View(policyTypesVM);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PolicyTypeCreateVM policyTypeCreateVM)
        {
            var policyType = new PolicyType
            {
                Name = policyTypeCreateVM.Name,
                Description = policyTypeCreateVM.Description,
                Condition = policyTypeCreateVM.Condition,
            };

            await dbContext.TypeOfPolicies.AddAsync(policyType);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(long id)
        {
            var policyType = await dbContext.TypeOfPolicies.FindAsync(id);

            if (policyType == null)
            {
                return NotFound();
            }

            return View(policyType);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(PolicyTypeVM policyTypeUpdateVM)
        {
            var policyType = await dbContext.TypeOfPolicies.FindAsync(policyTypeUpdateVM.Id);

            if (policyType == null)
            {
                return NotFound();
            }

            policyType.Name = policyTypeUpdateVM.Name;
            policyType.Description = policyTypeUpdateVM.Description;
            policyType.Condition = policyTypeUpdateVM.Condition;

            policyType.Risks = policyTypeUpdateVM.Risks.Select(rVM => new Risk
            {
                Id = rVM.Id,
                Name = rVM.Name,
                Description = rVM.Description,
                AverageProbability = rVM.AverageProbability,
            }).ToList();

            dbContext.TypeOfPolicies.Update(policyType);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(long id)
        {
            var policyType = await dbContext.TypeOfPolicies.FindAsync(id);

            if (policyType == null)
            {
                return NotFound();
            }

            dbContext.TypeOfPolicies.Remove(policyType);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}

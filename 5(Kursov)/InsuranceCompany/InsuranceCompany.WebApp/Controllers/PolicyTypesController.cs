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
    [Authorize(Roles = "Employee,Admin")]
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
            var policyType = await dbContext.TypeOfPolicies
                .Include(p => p.Risks)
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == id);

            if (policyType == null)
            {
                return NotFound();
            }

            var policyTypeVM = new PolicyTypeVM
            {
                Id = id,
                Name = policyType.Name,
                Condition = policyType.Condition,
                Description = policyType.Description,
                Risks = policyType.Risks.Select(r => new RiskVM
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    AverageProbability = r.AverageProbability,
                }).ToList(),
            };

            return View(policyTypeVM);
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

        public async Task<ActionResult> EditPolicyTypeRisks(long id)
        {
            var policyType = await dbContext.TypeOfPolicies
                .Include(p => p.Risks)
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == id);

            if (policyType == null)
            {
                return NotFound();
            }

            var policyTypeRisksUpdateVM = new List<PolicyTypeUpdateRiskVM>();
            foreach(var risk in dbContext.Risks)
            {
                var policyTypeRiskUpdateVM = new PolicyTypeUpdateRiskVM
                {
                    RiskId = risk.Id,
                    RiskName = risk.Name
                };

                policyTypeRiskUpdateVM.IsSelected = policyType.Risks.Any(r => r.Id == risk.Id);

                policyTypeRisksUpdateVM.Add(policyTypeRiskUpdateVM);
            }

            return View(policyTypeRisksUpdateVM);
        }

        [HttpPost]
        public async Task<ActionResult> EditPolicyTypeRisks(long id, List<PolicyTypeUpdateRiskVM> policyTypeRisksUpdateVM)
        {
            var policyType = await dbContext.TypeOfPolicies
                .Include(p => p.Risks)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (policyType == null)
            {
                return NotFound();
            }

            var riskIds = policyTypeRisksUpdateVM.Where(p => p.IsSelected).Select(p => p.RiskId);
            var risks = dbContext.Risks.Where(r => riskIds.Any(id => r.Id == id)).ToList();
            policyType.Risks = risks;

            dbContext.TypeOfPolicies.Update(policyType);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}

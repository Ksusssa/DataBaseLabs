using InsuranceCompany.Data.Internal.Contexts;
using InsuranceCompany.Data.Internal.Entities;
using InsuranceCompany.WebApp.Controllers.Base;
using InsuranceCompany.WebApp.Models.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCompany.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PoliciesController : BaseController
    {
        public PoliciesController(InsuranceCompanyDbContext dbContext) : base(dbContext)
        {
        }

        [ResponseCache(CacheProfileName = "LabaCacheProfile")]
        public async Task<ActionResult> Index()
        {
            var policies = await dbContext.Policies
                .Include(p => p.Employee)
                    .ThenInclude(e => e.User)
                .Include(p => p.Client)
                    .ThenInclude(c => c.User)
                .Include(p => p.PolicyType)
                .AsNoTracking()
                .ToListAsync();

            return View(policies);
        }

        public ActionResult Create()
        {
            var selectListEmpl = dbContext.Employees
                .Include(e => e.User)
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.User.UserName
                }).ToList();

            var selectListPolicies = dbContext.TypeOfPolicies
                .Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList();

            ViewBag.PolicyTypes = selectListPolicies;
            ViewBag.Employees = selectListEmpl;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(PolicyVM policyCreateVM)
        {
            var policy = new Policy
            {
                Payment = policyCreateVM.Payment,
                PolicyTypeId = policyCreateVM.PolicyTypeId,
                EmployeeId = policyCreateVM.EmployeeId,
                FinishDate = policyCreateVM.FinishDate,
                StartDate = policyCreateVM.StartDate,
                Price = policyCreateVM.Price,
            };

            await dbContext.Policies.AddAsync(policy);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(long id)
        {
            var policy = await dbContext.Policies.FindAsync(id);

            if (policy == null)
            {
                return NotFound();
            }

            dbContext.Policies.Remove(policy);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(long id)
        {
            var selectListEmpl = dbContext.Employees
                .Include(e => e.User)
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.User.UserName
                }).ToList();

            var selectListPolicies = dbContext.TypeOfPolicies
                .Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList();

            ViewBag.PolicyTypes = selectListPolicies;
            ViewBag.Employees = selectListEmpl;

            var policy = await dbContext.Policies.FindAsync(id);

            if (policy == null)
            {
                return NotFound();
            }

            var policyVM = new PolicyVM
            {
                Id = policy.Id,
                Payment = policy.Payment,
                PolicyTypeId = policy.PolicyTypeId,
                EmployeeId = policy.EmployeeId,
                FinishDate = policy.FinishDate,
                StartDate = policy.StartDate,
                Price = policy.Price,
            };

            return View(policyVM);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(PolicyVM policyVM)
        {
            var policy = await dbContext.Policies.FindAsync(policyVM.Id);

            if (policy == null)
            {
                return NotFound();
            }

            policy.Payment = policyVM.Payment;
            policy.Price = policyVM.Price;
            policy.PolicyTypeId = policyVM.PolicyTypeId;
            policy.EmployeeId = policyVM.EmployeeId;
            policy.FinishDate = policyVM.FinishDate;
            policy.StartDate = policyVM.StartDate;

            dbContext.Policies.Update(policy);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}

using InsuranceCompany.Data.Internal.Contexts;
using InsuranceCompany.Data.Internal.Entities;
using InsuranceCompany.WebApp.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InsuranceCompany.WebApp.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly UserManager<User> userManager;

        public HomeController(
            InsuranceCompanyDbContext dbContext,
            UserManager<User> userManager) : base(dbContext)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Client,Employee")]
        public async Task<ActionResult> Privacy()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user  = await userManager.FindByIdAsync(userId);
            var roles = await userManager.GetRolesAsync(user);

            var policies = dbContext.Policies
                .AsNoTracking()
                .Include(p => p.PolicyType)
                .Include(p => p.Client)
                    .ThenInclude(c => c.User)
                .Include(p => p.Employee)
                    .ThenInclude(e => e.User);

            if (roles.Any(r => r == "Client"))
            {
                return View(await policies
                    .Where(p => p.Client != null &&
                        p.Client.User.Id == userId)
                    .ToListAsync());
            }
            else if (roles.Any(r => r == "Employee"))
            {
                return View(await policies
                    .Where(p => p.Employee != null &&
                        p.Employee.User.Id == userId)
                    .ToListAsync());
            }

            return View();
        }

        [Authorize(Roles = "Client")]
        public ActionResult SelectPolicies()
        {
            var avaiblePolicies = dbContext.Policies
                .AsNoTracking()
                .Include(p => p.PolicyType)
                .Where(policy => !policy.IsFinish
                    && policy.ClientId == null)
                .ToList();

            return View(avaiblePolicies);
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult> SelectPolicy(long id)
        {
            var policy = await dbContext.Policies.FindAsync(id);
            if (policy == null) return NotFound();

            var client = dbContext.Clients.FirstOrDefault(
                c => c.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (client == null) return NotFound();

            policy.ClientId = client.Id;
            policy.IsPaied = true;

            dbContext.Policies.Update(policy);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Privacy");
        }
    }
}
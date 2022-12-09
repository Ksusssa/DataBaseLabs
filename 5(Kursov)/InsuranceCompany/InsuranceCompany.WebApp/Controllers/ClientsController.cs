using InsuranceCompany.Data.Internal.Entities;
using InsuranceCompany.Data.Internal.Contexts;
using InsuranceCompany.WebApp.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using InsuranceCompany.WebApp.Models.Client;
using Microsoft.AspNetCore.Identity;

namespace InsuranceCompany.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClientsController : BaseController
    {
        private readonly UserManager<User> userManager;

        public ClientsController(InsuranceCompanyDbContext dbContext, UserManager<User> userManager) 
            : base(dbContext)
        {
            this.userManager = userManager;
        }

        [ResponseCache(CacheProfileName = "LabaCacheProfile")]
        public async Task<ActionResult> Index()
        {
            var clients = await dbContext.Clients
                .Include(c => c.User)
                .Include(c => c.Policies)
                .ThenInclude(p => p.PolicyType)
                .AsNoTracking()
                .ToListAsync();

            return View(clients);
        }

        public async Task<ActionResult> Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ClientCreateVM clientCreateVM)
        {
            var user = new User 
            {
                Email = clientCreateVM.Email,
                UserName = clientCreateVM.Email,
                FirstName = clientCreateVM.FirstName,
                LastName = clientCreateVM.LastName,
                MiddleName = clientCreateVM.MiddleName,
                Gender = clientCreateVM.Gender,
            };

            var result = await userManager.CreateAsync(user, clientCreateVM.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            var client = new Client
            {
                UserId = user.Id,
                BornDate = clientCreateVM.BornDate,
                Address = clientCreateVM.Address,
                Passport = clientCreateVM.Passport,
                Phone = clientCreateVM.Phone
            };

            await dbContext.Clients.AddAsync(client);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public async Task<ActionResult> EditPolicies(long id)
        {
            var client = await dbContext.Clients
                .AsNoTracking()
                .Include(c => c.Policies)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            var policies = dbContext.Policies
                .Include(p => p.PolicyType)
                .AsNoTracking()
                .ToList()
                .Select(p => new ClientPoliciesUpdateVM
                {
                    PolicyId = p.Id,
                    PolicyName = p.PolicyType.Name,
                    IsSelected = client.Policies.Any(pol => pol.Id == p.Id),
                })
                .ToList();

            return View(policies);
        }

        [HttpPost]
        public async Task<ActionResult> EditPolicies(long id, List<ClientPoliciesUpdateVM> clientPoliciesVM)
        {
            var client = await dbContext.Clients
                .Include(c => c.Policies)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            var selectedIds = clientPoliciesVM
                .Where(cP => cP.IsSelected)
                .Select(cP => cP.PolicyId);

            client.Policies = dbContext.Policies
                .Where(p => selectedIds.Any(id => p.Id == id))
                .ToList();

            dbContext.Clients.Update(client);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(long id)
        {
            var client = await dbContext.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            dbContext.Clients.Remove(client);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}

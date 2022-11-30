using InsuranceCompany.Data.Internal.Entities;
using InsuranceCompany.Data.Internal.Contexts;
using InsuranceCompany.WebApp.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace InsuranceCompany.WebApp.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ClientsController : BaseController
    {
        public ClientsController(InsuranceCompanyDbContext dbContext) 
            : base(dbContext)
        {
        }

        [ResponseCache(CacheProfileName = "LabaCacheProfile")]
        public async Task<ActionResult> Index()
        {
            var clients = await dbContext.Clients
                .Include(c => c.Policies)
                .ThenInclude(p => p.PolicyType)
                .AsNoTracking()
                .ToListAsync();

            return View(clients);
        }

        public async Task<ActionResult> Details(long id)
        {
            var client = await dbContext.Clients
                .Include(c => c.Policies)
                .ThenInclude(p => p.PolicyType)
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(string name, int age)
        {
            var client = new Client
            {
                FirstName = name,
                BornDate = DateTime.Now
            };

            await dbContext.Clients.AddAsync(client);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public async Task<ActionResult> Edit(long id)
        {
            var client = await dbContext.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(long clientId, string name, int age)
        {
            var client = await dbContext.Clients.FindAsync(clientId);

            if(client == null)
            {
                return NotFound();
            }

            client.FirstName = name;

            dbContext.Clients.Update(client);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
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

using lab6.WebApplication.Contexts;
using Lab6.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab6_1.Controllers
{
    [Route("api/clients")]
    public class ClientController:Controller
    {
        private InsuranceCompanyDbContext db;

        public ClientController(InsuranceCompanyDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IEnumerable<Client> Get()
        {
            return db.Clients.Include(c => c.User).AsEnumerable();
        }

        [HttpGet("{id}")]
        public Client? Get(int id)
        {
            return db.Clients.Include(c => c.User).FirstOrDefault(c => c.Id == id);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Client client)
        {
            if (client != null)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return Ok();
            }
            else
                return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Client? client = db.Clients.FirstOrDefault(c => c.Id == id);
            if(client == null)
            {
                return NotFound();
            }
            else
            {
                db.Clients.Remove(client);
                db.SaveChanges();
                return Ok();
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] Client client)
        {
            if(db.Clients.Count(i=>i.Id == client.Id) == 0)
                return BadRequest();
            else
            {
                db.Clients.Update(client);
                db.SaveChanges();
                return Ok();
            }
        }
    }
}

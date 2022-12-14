using lab6.WebApplication.Contexts;
using Lab6.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab6_1.Controllers
{
    [Route("api/Policys")]
    public class PolicyController : Controller
    {
        InsuranceCompanyDbContext db;
        public PolicyController(InsuranceCompanyDbContext db)
        {
            this.db = db;
        }


        [HttpGet]
        public IEnumerable<Policy> Get()
        {
            return db.Policies.AsEnumerable();
        }

        //Get api/policys/5
        [HttpGet("{id}")]
        public Policy? Get(int id)
        {
            return db.Policies.FirstOrDefault(e => e.Id == id);
        }

        // POST api/policys
        [HttpPost]
        public IActionResult Add([FromBody] Policy policy)
        {
            if (policy == null || policy.Client == null || policy.Employee == null || policy.PolicyType == null)
            {
                return BadRequest();
            }
            else
            {
                 db.Policies.Add(policy);
                db.SaveChanges();
                return Ok(policy);
            }

           
        }

        // PUT api/policys/
        [HttpPut]
        public IActionResult Update([FromBody] Policy policy)
        {
            if (db.Policies.Count(e => e.Id == policy.Id) == 0)
                return BadRequest();
            else
            {
                db.Update(policy);
                db.SaveChanges();
                return Ok(policy);
            }
            
        }

        // DELETE api/policys/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Policy? policy = db.Policies.FirstOrDefault(x => x.Id == id);
            if (policy == null)
            {
                return NotFound();
            }
            else
            {
                db.Policies.Remove(policy);
                db.SaveChanges();
                return Ok(policy);
            }
            
        }

    }
}
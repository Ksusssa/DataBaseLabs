using lab6.WebApplication.Contexts;
using Lab6.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab6_1.Controllers
{
    [Route("api/TypePolicy")]
    public class TypePolicy : Controller
    {
        private readonly InsuranceCompanyDbContext db;

        public TypePolicy(InsuranceCompanyDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IEnumerable<PolicyType> Get()
        {
            return db.TypeOfPolicies.AsEnumerable();
        }

        [HttpGet("{id}")]
        public PolicyType? Get(int id)
        {
            return db.TypeOfPolicies.FirstOrDefault(x => x.Id == id);
        }

        [HttpPost]
        public IActionResult Add(PolicyType policyType)
        {
            if (policyType != null)
            {
                db.TypeOfPolicies.Add(policyType);
                db.SaveChanges();
                return Ok();
            }
            else
                return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            PolicyType? policyType = db.TypeOfPolicies.FirstOrDefault(x => x.Id == id);
            if(policyType == null)
            {
                return NotFound();
            }
            else
            {
                db.TypeOfPolicies.Remove(policyType);
                db.SaveChanges();
                return Ok();
            }
        }

        [HttpPut]
        public IActionResult Update(PolicyType policyType)
        {
            if(db.TypeOfPolicies.Count(i=>i.Id == policyType.Id) == 0)
            {
                return BadRequest();
            }
            else
            {
                db.TypeOfPolicies.Update(policyType);
                db.SaveChanges();
                return Ok();
            }
        }
    }
}

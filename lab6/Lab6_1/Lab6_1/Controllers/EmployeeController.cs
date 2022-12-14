using lab6.WebApplication.Contexts;
using Lab6.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab6_1.Controllers
{
    [Route("api/Employees")]
    public class EmployeeController: Controller
    {
        private InsuranceCompanyDbContext db;
        public EmployeeController(InsuranceCompanyDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            return db.Employees.Include(e => e.User).AsEnumerable();
        }

        [HttpGet("{id}")]
        public Employee? Get(int id)
        {
            return db.Employees.Include(e => e.User).FirstOrDefault(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult Add (Employee employee)
        {
            if(employee != null)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Employee? employee = db.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
                return NotFound();
            else
            {
                db.Employees.Remove(employee);
                db.SaveChanges();
                return Ok();
            }
        }

        [HttpPut]
        public IActionResult Update(Employee employee)
        {
            if(db.Employees.Count(i => i.Id == employee.Id) == 0)
            {
                return BadRequest();
            }
            else
            {
                db.Employees.Update(employee);
                db.SaveChanges();
                return Ok();
            }
        }
    }
}

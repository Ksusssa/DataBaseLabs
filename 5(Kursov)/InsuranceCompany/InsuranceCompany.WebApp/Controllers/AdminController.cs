using InsuranceCompany.Data.Internal.Contexts;
using InsuranceCompany.Data.Internal.Entities;
using InsuranceCompany.WebApp.Controllers.Base;
using InsuranceCompany.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceCompany.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly UserManager<User> userManager;

        public AdminController(UserManager<User> userManager, InsuranceCompanyDbContext dbContext) : base(dbContext)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Users()
        {
            var clients = await userManager.GetUsersInRoleAsync("Client");
            var employees = await userManager.GetUsersInRoleAsync("Employee");

            return Json(clients.Concat(employees).Select(u => new UserVM
            {
                Id = u.Id,
                UserName = u.UserName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                MiddleName = u.MiddleName,
            }));
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUser(UserVM userVM)
        {
            var user = await userManager.FindByIdAsync(userVM.Id);

            user.UserName = userVM.UserName;
            user.PhoneNumber = userVM.PhoneNumber;
            user.Email = userVM.Email;
            user.FirstName = userVM.FirstName;
            user.LastName = userVM.LastName;
            user.MiddleName = userVM.MiddleName;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
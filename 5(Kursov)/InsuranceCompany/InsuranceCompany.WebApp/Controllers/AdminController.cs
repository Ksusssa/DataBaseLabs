using Microsoft.AspNetCore.Mvc;

namespace InsuranceCompany.WebApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

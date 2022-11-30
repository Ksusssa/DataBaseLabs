using InsuranceCompany.Data.Internal.Contexts;
using InsuranceCompany.WebApp.Controllers.Base;

namespace InsuranceCompany.WebApp.Controllers
{
    public class EmployeeController : BaseController
    {
        public EmployeeController(InsuranceCompanyDbContext dbContext) : base(dbContext)
        {
        }


    }
}

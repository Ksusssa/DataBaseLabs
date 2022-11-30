using InsuranceCompany.Data.Internal.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceCompany.WebApp.Controllers.Base
{
    public abstract class BaseController : Controller
    {
        protected readonly InsuranceCompanyDbContext dbContext;

        protected BaseController(InsuranceCompanyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

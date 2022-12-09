using InsuranceCompany.Data.Internal.Contexts;
using InsuranceCompany.Data.Internal.Entities;
using InsuranceCompany.Data.Internal.Helpers;
using Microsoft.AspNetCore.Identity;

namespace InsuranceCompany.WebApp.Middlewares
{
    public class DbInitializeMiddleware
    {
        private readonly RequestDelegate _next;

        public DbInitializeMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var dbContext = context.RequestServices.GetRequiredService<InsuranceCompanyDbContext>();
            var userManager = context.RequestServices.GetRequiredService<UserManager<User>>();
            var roleManager = context.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();

            if (dbContext == null)
            {
                await _next(context);
                return;
            }

            await DbInitializer.InitializeRoles(userManager, roleManager);
            await DbInitializer.InitializeUsers(dbContext, userManager);

            DbInitializer.InitializeData(dbContext);
           
            await _next(context);
        }
    }
}

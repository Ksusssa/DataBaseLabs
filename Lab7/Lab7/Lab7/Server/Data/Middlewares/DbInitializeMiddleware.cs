
using InsuranceCompany.Data.Internal.Helpers;
using lab6.WebApplication.Contexts;
using Lab6.WebApplication.Models;
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

            await DbInitializerMiddlewares.InitializeRoles(userManager, roleManager);
            await DbInitializerMiddlewares.InitializeUsers(dbContext, userManager);

            DbInitializerMiddlewares.InitializeData(dbContext);
           
            await _next(context);
        }
    }
}

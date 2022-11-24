using InsuranceCompany.Data.Internal.Contexts;
using InsuranceCompany.Data.Internal.Helpers;

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

            if (dbContext == null)
            {
                await _next(context);
            }

            DbInitializer.Initialize(dbContext);
            await _next(context);
        }
    }
}

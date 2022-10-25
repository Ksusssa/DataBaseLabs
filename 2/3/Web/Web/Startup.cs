using InsuranceLib.Data;
using System.Text;
using Web.Middleware;
using Web.Services;

namespace Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<InsuranceContext>();
            services.AddTransient<PolicyService>();
            services.AddTransient<EmployeeService>();
            services.AddMemoryCache();
            services.AddControllersWithViews();
            services.AddSession();
        }

        public void Configure(IHostEnvironment environment, IApplicationBuilder app,PolicyService policyService, EmployeeService employeeService)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSession();
            app.UseMiddleware<ButtonHadlerMiddleware>();
            app.UseMiddleware<ButtonHadlerMiddleware1>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", (context) =>
                {
                    var builder = new StringBuilder();
                    builder.Append("<H1> Main page</H1>");
                    builder.Append(@"<a href = '/info'> Info about user </a></br>");
                    builder.Append(@"<a href = '/Policy'> Policy table </a></br>");
                    builder.Append(@"<a href = '/Policy/Search'> Search Policy with cookie</a></br>");
                    builder.Append(@"<a href = '/Policy/SearchSes'> Search Policy with session</a></br>");

                    return context.Response.WriteAsync(builder.ToString());
                });

                #region PolicyTable
                endpoints.MapGet("/Policy", (context) =>
                {
                    var builder = new StringBuilder();

                    builder.Append("<div>");
                    builder.Append("<H1>Policy table</H1>");
                    builder.Append("<table border = 1>");
                    builder.Append("<tr>");
                    builder.Append($"<td> StartDate </td>"+
                                    $"<td>FinishDate </td>"+
                                    $"<td> Price </td>"+
                                    $"<td> Employee</td>");
                    builder.Append("</tr>");

                    foreach (var policy in policyService.GetFromCach(20, "allPolicys"))
                    {
                        builder.Append("<tr>");
                        builder.Append($"<td> {policy.StartDate.ToString("dd.MM.yyyy")} </td>" +
                                        $"<td> {policy.FinishDate.ToString("dd.MM.yyyy")} </td>" +
                                        $"<td> {policy.Price} </td>" +
                                        $"<td> {policy.Employee.MiddleName} </td>");
                        builder.Append("</tr>");
                    }
                    builder.Append("</table>");
                    builder.Append("</div>");
                    return context.Response.WriteAsync(builder.ToString());
                });
                #endregion

                endpoints.MapGet("/info",(context) =>
                {
                    string browser = context.Request.Headers["sec-ch-ua"];
                    string platform = context.Request.Headers["sec-ch-ua-platform"];

                    return context.Response.WriteAsync("<p>Browser: " + browser + "</p><p>Platform: " + platform);

                });
                endpoints.MapControllers();
            });
        }
    }
}

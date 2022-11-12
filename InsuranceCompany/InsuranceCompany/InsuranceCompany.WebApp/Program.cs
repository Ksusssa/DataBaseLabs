using InsuranceCompany.Data.Internal.Contexts;
using InsuranceCompany.WebApp.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
//TO-DO: Create web-core project, extract logic of registration, move dependency from data.internal to web-core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<InsuranceCompanyDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<InsuranceCompanyDbContext>();

builder.Services.AddControllersWithViews(options =>
{
    options.CacheProfiles.Add("LabaCacheProfile", new Microsoft.AspNetCore.Mvc.CacheProfile
    {
        Duration = 304,
        Location = Microsoft.AspNetCore.Mvc.ResponseCacheLocation.Any,
        NoStore = false
    });
});
builder.Services.AddResponseCaching();

var app = builder.Build();

app.UseMiddleware<DbInitializeMiddleware>();
app.UseResponseCaching();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

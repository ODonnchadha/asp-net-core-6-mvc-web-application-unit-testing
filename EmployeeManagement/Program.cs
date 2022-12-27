using EmployeeManagement;
using EmployeeManagement.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("TopLevelManagementAPIClient");

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.RegisterBusinessServices();
builder.Services.RegisterDataServices(builder.Configuration);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

var app = builder.Build();

app.UseMiddleware<EmployeeManagementSecurityHeadersMiddleware>();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=EmployeeOverview}/{action=Index}/{id?}");

app.Run();
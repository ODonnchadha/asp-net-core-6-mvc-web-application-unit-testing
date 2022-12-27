using EmployeeManagement.Contexts;
using EmployeeManagement.Factories;
using EmployeeManagement.Interfaces.Repositories;
using EmployeeManagement.Interfaces.Services;
using EmployeeManagement.Repositories;
using EmployeeManagement.Services;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection RegisterBusinessServices(
            this IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<EmployeeFactory>(); 
            return services;
        }

        public static IServiceCollection RegisterDataServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EmployeeDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("EmployeeManagementDB")));
            services.AddScoped<IEmployeeManagementRepository, EmployeeManagementRepository>();
            return services;
        }
    }
}

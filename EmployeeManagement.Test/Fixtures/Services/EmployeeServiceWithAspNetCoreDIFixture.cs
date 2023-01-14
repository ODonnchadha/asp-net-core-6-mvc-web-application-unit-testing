using EmployeeManagement.Factories;
using EmployeeManagement.Interfaces.Repositories;
using EmployeeManagement.Interfaces.Services;
using EmployeeManagement.Repositories.Tests;
using EmployeeManagement.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Test.Fixtures.Services
{
    public class EmployeeServiceWithAspNetCoreDIFixture : IDisposable
    {
        private readonly ServiceProvider provider;

        public IEmployeeManagementRepository EmployeeManagementTestDataRepository
        {
            get
            {
#pragma warning disable CS8603 // Possible null reference return.
                return provider.GetService<IEmployeeManagementRepository>();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

        public IEmployeeService EmployeeService
        {
            get
            {
#pragma warning disable CS8603 // Possible null reference return.
                return provider.GetService<IEmployeeService>();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }
        public EmployeeServiceWithAspNetCoreDIFixture()
        {
            var services = new ServiceCollection { };

            services.AddScoped<EmployeeFactory>();
            services.AddScoped<
                IEmployeeManagementRepository, EmployeeManagementTestDataRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            this.provider = services.BuildServiceProvider();
        }
        public void Dispose() { }
    }
}

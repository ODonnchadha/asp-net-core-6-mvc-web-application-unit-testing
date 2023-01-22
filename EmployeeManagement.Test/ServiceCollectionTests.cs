using EmployeeManagement.Interfaces.Repositories;
using EmployeeManagement.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EmployeeManagement.Test
{
    public class ServiceCollectionTests
    {
        [Fact()]
        public void RegisterDataServices_Execute_DataServicesAreRegistered()
        {
            // Arrange.
            var collection = new ServiceCollection { };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    { 
                        "ConnectionStrings:EmployeeManagementDB", "X"
                    }
                }).Build();

            // Act.
            collection.RegisterDataServices(configuration);

            var provider = collection.BuildServiceProvider();

            // Assert.
            Assert.NotNull(provider.GetService<IEmployeeManagementRepository>());
            Assert.IsType<EmployeeManagementRepository>(
                provider.GetService<IEmployeeManagementRepository>());
        }
    }
}

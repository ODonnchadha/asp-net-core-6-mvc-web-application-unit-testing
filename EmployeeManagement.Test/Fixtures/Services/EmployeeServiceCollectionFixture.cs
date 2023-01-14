using Xunit;

namespace EmployeeManagement.Test.Fixtures.Services
{
    [CollectionDefinition("EmployeeServiceCollection")]
    public class EmployeeServiceCollectionFixture 
        : ICollectionFixture<EmployeeServiceFixture> { }
}

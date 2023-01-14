using EmployeeManagement.Factories;
using EmployeeManagement.Interfaces.Repositories;
using EmployeeManagement.Repositories.Tests;
using EmployeeManagement.Services;

namespace EmployeeManagement.Test.Fixtures.Services
{
    public class EmployeeServiceFixture : IDisposable
    {
        public IEmployeeManagementRepository EmployeeManagementTestDataRepository { get; }
        public EmployeeService EmployeeService { get; }

        public EmployeeServiceFixture()
        {
            EmployeeManagementTestDataRepository = new EmployeeManagementTestDataRepository { };
            EmployeeService = new EmployeeService(
                new EmployeeFactory(), EmployeeManagementTestDataRepository);
        }
        public void Dispose() { }
    }
}

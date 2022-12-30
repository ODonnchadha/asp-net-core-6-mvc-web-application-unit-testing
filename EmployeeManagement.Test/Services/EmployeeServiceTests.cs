using EmployeeManagement.Factories;
using EmployeeManagement.Repositories.Tests;
using EmployeeManagement.Services;
using Xunit;

namespace EmployeeManagement.Test.Services
{
    public class EmployeeServiceTests
    {
        [Fact()]
        public void CreateInternalEmployee_InternalEmployeeCreated_MastHaveAttendedFirstObligatoryClass()
        {
            // Arrange.
            var repository = new EmployeeManagementTestDataRepository { };
            var service = new EmployeeService(
                new EmployeeFactory(), repository);

            // Act.
            var employee = service.CreateInternalEmployee("INTERNAL", "EMPLOYEE");

            // Assert.
            Assert.Contains(
                repository.GetCourse(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01")), 
                employee.AttendedCourses);
        }
    }
}

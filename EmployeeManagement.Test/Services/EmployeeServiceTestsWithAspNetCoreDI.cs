using EmployeeManagement.Test.Fixtures.Services;
using Xunit;

namespace EmployeeManagement.Test.Services
{
    public class EmployeeServiceTestsWithAspNetCoreDI : IClassFixture<EmployeeServiceWithAspNetCoreDIFixture>
    {
        private readonly EmployeeServiceWithAspNetCoreDIFixture fixture;

        public EmployeeServiceTestsWithAspNetCoreDI(EmployeeServiceWithAspNetCoreDIFixture fixture) =>
            this.fixture = fixture;

        [Fact()]
        public void CreateInternalEmployee_InternalEmployeeCreated_MastHaveAttendedFirstObligatoryClass()
        {
            // Act.
            var employee =
                fixture.EmployeeService.CreateInternalEmployee("INTERNAL", "EMPLOYEE");

            // Assert.
            Assert.Contains(
                fixture.EmployeeManagementTestDataRepository.GetCourse(
                    Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01")),
                employee.AttendedCourses);

            // Assert.
            Assert.Contains(employee.AttendedCourses,
                c => c.Id == Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
        }
    }
}

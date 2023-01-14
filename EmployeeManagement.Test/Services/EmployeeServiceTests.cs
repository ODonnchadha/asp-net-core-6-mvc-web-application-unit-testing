using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.Entities;
using Xunit;

namespace EmployeeManagement.Test.Services
{
    public class EmployeeServiceTests : IClassFixture<Fixtures.Services.EmployeeServiceFixture>
    {
        private readonly Fixtures.Services.EmployeeServiceFixture fixture;

        public EmployeeServiceTests(
            Fixtures.Services.EmployeeServiceFixture fixture) => this.fixture = fixture;

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

        [Fact()]
        public void CreateInternalEmployee_InternalEmployeeCreated_MastHaveAttendedSecondObligatoryClass()
        {
            // Arrange.
            var employee = 
                fixture.EmployeeService.CreateInternalEmployee("INTERNAL", "EMPLOYEE");

            // Assert.
            Assert.Contains(employee.AttendedCourses,
                c => c.Id == Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));
        }

        [Fact()]
        public void CreateInternalEmployee_InternalEmployeeCreated_MastHaveAttendedObligatoryClasses()
        {
            // Arrange.
            var employee = 
                fixture.EmployeeService.CreateInternalEmployee("INTERNAL", "EMPLOYEE");

            var courses = fixture.EmployeeManagementTestDataRepository.GetCourses(
                Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            // Assert.
            Assert.Equal(employee.AttendedCourses, courses);
        }

        [Fact()]
        public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustNotBeNew()
        {
            // Arrange.
            var employee = 
                fixture.EmployeeService.CreateInternalEmployee("INTERNAL", "EMPLOYEE");

            // Assert.
            employee.AttendedCourses.ForEach(c => Assert.False(c.IsNew));
            Assert.All(employee.AttendedCourses, c => Assert.False(c.IsNew));
        }

        [Fact()]
        public async Task CreateInternalEmployee_InternalEmployeeCreated_MastHaveAttendedObligatoryCourses_Async()
        {
            // Arrange.
            var employee = 
                await fixture.EmployeeService.CreateInternalEmployeeAsync(
                    "INTERNAL", "EMPLOYEE");

            var courses = await fixture.EmployeeManagementTestDataRepository
                .GetCoursesAsync(
                    Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                    Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            // Assert.
            Assert.Equal(employee.AttendedCourses, courses);
        }

        [Fact()]
        public async Task GiveRaise_RaiseBelowMinimumGiven_EmployeeInvalidRaiseExceptionMustBeThrown_Async()
        {
            // Arrange.
            var employee = new InternalEmployee(
                "INTERNAL", "EMPLOYEE", 5, 3000, false, 1);

            // Act. Assert.
            await Assert.ThrowsAsync<EmployeeInvalidRaiseException>(
                async () => await fixture.EmployeeService.GiveRaiseAsync(employee, 50));
        }

        [Fact()]
        public void NotifyOfAbsence_EmployeeIsAbsent_OnEmployeeIsAbsentMustBeTriggered()
        {
            // Arrange.
            var employee = new InternalEmployee(
                "INTERNAL", "EMPLOYEE", 5, 3000, false, 1);

            // Act. Assert.
            Assert.Raises<EmployeeIsAbsentEventArgs>(
                handler => fixture.EmployeeService.EmployeeIsAbsent += handler,
                handler => fixture.EmployeeService.EmployeeIsAbsent -= handler,
                () => fixture.EmployeeService.NotifyOfAbsence(employee));
        }
    }
}

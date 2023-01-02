using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.Entities;
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

            // Assert.
            Assert.Contains(employee.AttendedCourses, 
                c => c.Id == Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
        }

        [Fact()]
        public void CreateInternalEmployee_InternalEmployeeCreated_MastHaveAttendedSecondObligatoryClass()
        {
            // Arrange.
            var employee = new EmployeeService(
                    new EmployeeFactory(), 
                    new EmployeeManagementTestDataRepository()
                ).CreateInternalEmployee("INTERNAL", "EMPLOYEE");

            // Assert.
            Assert.Contains(employee.AttendedCourses,
                c => c.Id == Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));
        }

        [Fact()]
        public void CreateInternalEmployee_InternalEmployeeCreated_MastHaveAttendedObligatoryClasses()
        {
            // Arrange.
            var repository = new EmployeeManagementTestDataRepository { };
                
            var employee = new EmployeeService(new EmployeeFactory(), 
                repository).CreateInternalEmployee("INTERNAL", "EMPLOYEE");

            var courses = repository.GetCourses(
                Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            // Assert.
            Assert.Equal(employee.AttendedCourses, courses);
        }

        [Fact()]
        public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustNotBeNew()
        {
            // Arrange.
            var employee = new EmployeeService(
                    new EmployeeFactory(),
                    new EmployeeManagementTestDataRepository())
                .CreateInternalEmployee("INTERNAL", "EMPLOYEE");

            // Assert.
            employee.AttendedCourses.ForEach(c => Assert.False(c.IsNew));
            Assert.All(employee.AttendedCourses, c => Assert.False(c.IsNew));
        }

        [Fact()]
        public async Task CreateInternalEmployee_InternalEmployeeCreated_MastHaveAttendedObligatoryCourses_Async()
        {
            // Arrange.
            var repository = new EmployeeManagementTestDataRepository { };

            var employee = await new EmployeeService(new EmployeeFactory(),
                repository).CreateInternalEmployeeAsync("INTERNAL", "EMPLOYEE");

            var courses = await repository.GetCoursesAsync(
                Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            // Assert.
            Assert.Equal(employee.AttendedCourses, courses);
        }

        [Fact()]
        public async Task GiveRaise_RaiseBelowMinimumGiven_EmployeeInvalidRaiseExceptionMustBeThrown_Async()
        {
            // Arrange.
            var service = new EmployeeService(
                new EmployeeFactory(),
                new EmployeeManagementTestDataRepository());
               
            var employee = new InternalEmployee(
                "INTERNAL", "EMPLOYEE", 5, 3000, false, 1);

            // Act. Assert.
            await Assert.ThrowsAsync<EmployeeInvalidRaiseException>(
                async () => await service.GiveRaiseAsync(employee, 50));
        }

        [Fact()]
        public void NotifyOfAbsence_EmployeeIsAbsent_OnEmployeeIsAbsentMustBeTriggered()
        {
            // Arrange.
            var service = new EmployeeService(
                new EmployeeFactory(),
                new EmployeeManagementTestDataRepository());

            var employee = new InternalEmployee(
                "INTERNAL", "EMPLOYEE", 5, 3000, false, 1);

            // Act. Assert.
            Assert.Raises<EmployeeIsAbsentEventArgs>(
                handler => service.EmployeeIsAbsent += handler,
                handler => service.EmployeeIsAbsent -= handler,
                () => service.NotifyOfAbsence(employee));
        }
    }
}

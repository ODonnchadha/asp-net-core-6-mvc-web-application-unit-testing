using EmployeeManagement.Entities;
using EmployeeManagement.Factories;
using EmployeeManagement.Interfaces.Repositories;
using EmployeeManagement.Repositories.Tests;
using EmployeeManagement.Services;
using Moq;
using Xunit;

namespace EmployeeManagement.Test.Services
{
    public class MoqEmployeeServiceTests
    {
        [Fact()]
        public void FetchInternalEmployee_EmployeeFetched_SuggestedBonusMustBeCalculated()
        {
            // Arrange.
            var service = new EmployeeService(
                new Mock<EmployeeFactory>().Object, 
                new EmployeeManagementTestDataRepository { });

            // Act.
            var employee = service.FetchInternalEmployee(
                Guid.Parse("72f2f5fe-e50c-4966-8420-d50258aefdcb"));

            // Assert.
            Assert.Equal(400, employee?.SuggestedBonus);
        }

        [Fact()]
        public void CreateInternalEmployee_EmployeeCreated_SuggestedBonusMustBeCalculated()
        {
            // Arrange.
            decimal bonus = 1000;

            var factory = new Mock<EmployeeFactory>();
            factory.Setup(f => f.CreateEmployee(
                It.IsAny<string>(),
                It.IsAny<string>(),
                null, false)).Returns(new InternalEmployee(
                    "FIRSTNAME", "LASTNAME", 5, 2500, false, 1));

            var service = new EmployeeService(
                factory.Object,
                new EmployeeManagementTestDataRepository { });

            // Act.
            var employee = service.CreateInternalEmployee(
                "FIRSTNAME", "LASTNAME");

            // Assert.
            Assert.Equal(bonus, employee.SuggestedBonus);
        }

        [Fact()]
        public async Task FetchInternalEmployee_InternalEmployeeFetched_SuggestedBonusMustBeCalculated_Async()
        {
            // Arrange.
            var repository = new Mock<IEmployeeManagementRepository>();
            repository.Setup(r => r.GetInternalEmployeeAsync(It.IsAny<Guid>())).ReturnsAsync(
                new InternalEmployee(
                    "FIRSTNAME", "LASTNAME", 2, 2500, false, 2)
                {
                    AttendedCourses = new List<Course>
                    {
                        new Course("X")
                    }
                });

            var service = new EmployeeService(
                new Mock<EmployeeFactory>().Object, repository.Object);

            // Act.
            var employee =  await service.FetchInternalEmployeeAsync(Guid.Empty);

            // Assert.
            Assert.Equal(200, employee?.SuggestedBonus);
        }

        [Fact()]
        public void FetchInternalEmployee_InternalEmployeeFetched_SuggestedBonusMustBeCalculated()
        {
            // Arrange.
            var repository = new Mock<IEmployeeManagementRepository>();
            repository.Setup(r => r.GetInternalEmployee(It.IsAny<Guid>())).Returns(
                new InternalEmployee(
                    "FIRSTNAME", "LASTNAME", 2, 2500, false, 2)
                {
                    AttendedCourses = new List<Course>
                    {
                        new Course("X"), new Course("Y"), new Course("Z")
                    }
                });

            var service = new EmployeeService(
                new Mock<EmployeeFactory>().Object, repository.Object);

            // Act.
            var employee = service.FetchInternalEmployee(Guid.Empty);

            // Assert.
            Assert.Equal(600, employee?.SuggestedBonus);
        }
    }
}

using EmployeeManagement.Contexts;
using EmployeeManagement.Entities;
using EmployeeManagement.Factories;
using EmployeeManagement.Repositories;
using EmployeeManagement.Repositories.Tests;
using EmployeeManagement.Services;
using EmployeeManagement.Test.Handlers.HttpMessageHandlers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Sdk;

namespace EmployeeManagement.Test.Services
{
    public class IsolationApproachesEmployeeServiceTests
    {
        [Fact()]
        public async Task AttendCourseAsync_CourseAttended_SuggestedBonusMustCorrectlyBeRecalculated()
        {
            // Arrange.
            var conn = new SqliteConnection("Data Source=:memory:");
            conn.Open();

            var builder = 
                new DbContextOptionsBuilder<EmployeeDbContext>()
                .UseSqlite(conn);

            var context = new EmployeeDbContext(builder.Options);
            context.Database.Migrate();

            var repository = new EmployeeManagementRepository(context);
            var service = new EmployeeService(new EmployeeFactory { }, repository);

            // 'Dealing with Customers 101"
            var course = await repository.GetCourseAsync(
                Guid.Parse("844e14ce-c055-49e9-9610-855669c9859b"));
            // "Megan Jones"
            var employee = await repository.GetInternalEmployeeAsync(
                Guid.Parse("72f2f5fe-e50c-4966-8420-d50258aefdcb"));

            if (course == null || employee == null)
            {
                throw new XunitException("Arrange failed.");
            }

            var expectation = employee.YearsInService *
                (employee.AttendedCourses.Count + 1) * 100;

            // Act.
            await service.AttendCourseAsync(employee, course);

            // Assert.
            Assert.Equal(expectation, employee.SuggestedBonus);
        }

        [Fact()]
        public async Task PromoteInternalEmployeeAsync_IsEligible_JobLevelMustBeIncreased()
        {
            // Arrange.
            var client = new HttpClient(
                new PromotionEligibilityMessageHandler(true));
            var employee = new InternalEmployee(
                "FIRSTNAME", "LASTNAME", 5, 3000, false, 1);
            var service = new PromotionService(client, new EmployeeManagementTestDataRepository { });

            // Act.
            await service.PromoteInternalEmployeeAsync(employee);

            // Assert.
            Assert.Equal(2, employee.JobLevel);
        }
    }
}

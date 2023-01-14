using EmployeeManagement.Entities;
using Xunit;

namespace EmployeeManagement.Test.Services
{
    [Collection("EmployeeServiceCollection")]
    public class DataDrivenEmployeeServiceTests
    {
        private readonly Fixtures.Services.EmployeeServiceFixture fixture;

        public DataDrivenEmployeeServiceTests(
            Fixtures.Services.EmployeeServiceFixture fixture) => this.fixture = fixture;

        [Theory()]
        [InlineData("37e03ca7-c730-4351-834c-b66f280cdb01")]
        [InlineData("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e")]
        public void CreateInternalEmployee_InternalEmployeeCreated_MastHaveAttendedObligatoryClassed(Guid id)
        {
            // Act.
            var employee =
                fixture.EmployeeService.CreateInternalEmployee("INTERNAL", "EMPLOYEE");

            // Assert.
            Assert.Contains(
                fixture.EmployeeManagementTestDataRepository.GetCourse(id),
                employee.AttendedCourses);

            // Assert.
            Assert.Contains(employee.AttendedCourses, c => c.Id == id);
        }
        [Theory()]
        [ClassData(typeof(Data.Services.EmployeeServiceStronglyTypedTestDataFromFile))]
        public async Task GiveRaise_RaiseGiven_EmployeeRaiseGivenMatchesProvidedStronglyTypedFromExternal_Async(
            int raise, bool isRaise)
        {
            // Arrange.
            var employee = new InternalEmployee(
                "INTERNAL", "EMPLOYEE", 5, 3000, false, 1);

            // Act.
            await fixture.EmployeeService.GiveRaiseAsync(employee, raise);

            // Assert.
            Assert.Equal(isRaise, employee.MinimumRaiseGiven);
        }

        [Theory()]
        [ClassData(typeof(Data.Services.StronglyTypedEmployeeServiceTestData))]
        public async Task GiveRaise_RaiseGiven_EmployeeRaiseGivenMatchesProvidedStronglyTyped_Async(
            int raise, bool isRaise)
        {
            // Arrange.
            var employee = new InternalEmployee(
                "INTERNAL", "EMPLOYEE", 5, 3000, false, 1);

            // Act.
            await fixture.EmployeeService.GiveRaiseAsync(employee, raise);

            // Assert.
            Assert.Equal(isRaise, employee.MinimumRaiseGiven);
        }

        [Theory()]
        [ClassData(typeof(Data.Services.EmployeeServiceTestData))]
        [MemberData(nameof(ExampleTestDataFroGiveRaise_WithStronglyTypedData))]
        public async Task GiveRaise_RaiseGiven_EmployeeRaiseGivenMatchesProvidedClass_Async(
            int raise, bool isRaise)
        {
            // Arrange.
            var employee = new InternalEmployee(
                "INTERNAL", "EMPLOYEE", 5, 3000, false, 1);

            // Act.
            await fixture.EmployeeService.GiveRaiseAsync(employee, raise);

            // Assert.
            Assert.Equal(isRaise, employee.MinimumRaiseGiven);
        }

        public static TheoryData<int, bool> ExampleTestDataFroGiveRaise_WithStronglyTypedData
        {
            get
            {
                return new TheoryData<int, bool>
                {
                    { 100, true },
                    { 200, false }
                };
            }
        }
        public static IEnumerable<object[]> ExampleTestDataFroGiveRaise_WithProperty
        {
            get
            {
                return new List<object[]>
                {
                    new object[] { 100, true },
                    new object[] { 200, false }
                };
            }
        }

        public static IEnumerable<object[]> ExampleTestDataFroGiveRaise_WithMethod(
            int count)
        {
            var l = new List<object[]>
                {
                    new object[] { 100, true },
                    new object[] { 200, false }
                };

            return l.Take(count);
        }

        [Theory()]
        [MemberData(nameof(ExampleTestDataFroGiveRaise_WithProperty))]
        [MemberData(nameof(ExampleTestDataFroGiveRaise_WithMethod), 1)]
        public async Task GiveRaise_RaiseGiven_EmployeeRaiseGivenMatchesValue_Async(
            int raise, bool isRaise)
        {
            // Arrange.
            var employee = new InternalEmployee(
                "INTERNAL", "EMPLOYEE", 5, 3000, false, 1);

            // Act.
            await fixture.EmployeeService.GiveRaiseAsync(employee, raise);

            // Assert.
            Assert.Equal(isRaise, employee.MinimumRaiseGiven);
        }

        [Fact()]
        public async Task GiveRaise_MinimumRaiseGiven_EmployeeMinimumRaiseGivenMustBeTrue_Async()
        {
            // Arrange.
            var employee = new InternalEmployee(
                "INTERNAL", "EMPLOYEE", 5, 3000, false, 1);

            // Act.
            await fixture.EmployeeService.GiveRaiseAsync(employee, 100);

            // Assert.
            Assert.True(employee.MinimumRaiseGiven);
        }

        [Fact()]
        public async Task GiveRaise_MoreThanMinimumRaiseGiven_EmployeeMinimumRaiseGivenMustBeFalse_Async()
        {
            // Arrange.
            var employee = new InternalEmployee(
                "INTERNAL", "EMPLOYEE", 5, 3000, false, 1);

            // Act.
            await fixture.EmployeeService.GiveRaiseAsync(employee, 200);

            // Assert.
            Assert.False(employee.MinimumRaiseGiven);
        }
    }
}

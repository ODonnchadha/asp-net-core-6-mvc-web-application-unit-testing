using EmployeeManagement.Entities;
using EmployeeManagement.Factories;
using Xunit;

namespace EmployeeManagement.Test.Factories
{
    public class EmployeeFactoryTests : IDisposable
    {
        private EmployeeFactory factory;

        /// <summary>
        /// Test context is recreated per each and every single test.
        /// </summary>
        public EmployeeFactoryTests() => this.factory = new EmployeeFactory { };
        public void Dispose() { }

        [Fact(), Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBe2500()
        {
            var employee = (InternalEmployee)factory.CreateEmployee("FIRSTNAME", "LASTNAME");

            Assert.Equal(2500, employee.Salary);
        }

        [Fact(Skip = "Skipping"), Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeGreaterThanOrEqualTo2500()
        {
            var employee = (InternalEmployee)factory.CreateEmployee("FIRSTNAME", "LASTNAME");

            Assert.True(employee.Salary >= 2500);
        }

        [Fact(), Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500()
        {
            var employee = (InternalEmployee)factory.CreateEmployee("FIRSTNAME", "LASTNAME");

            //Assert.True(employee.Salary >= 2500 && employee.Salary <= 3500,
            //    $"Salary {employee.Salary} is not within an acceptable range,");
            Assert.InRange(employee.Salary, 2500, 3500);
        }

        [Fact(), Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500_Precesion()
        {
            var employee = (InternalEmployee)factory.CreateEmployee("FIRSTNAME", "LASTNAME");
            employee.Salary = 2500.123m;

            Assert.Equal(2500, employee.Salary, 0);
        }

        [Fact(), Trait("Category", "EmployeeFactory_CreateEmployee")]
        public void CreateEmployee_IsExternalIsTrue_ReturnTypeMustBeExternalEmployee()
        {
            var employee = factory.CreateEmployee("FIRSTNAME", "LASTNAME", "COMPANY", true);

            Assert.IsType<ExternalEmployee>(employee);
            Assert.IsAssignableFrom<Employee>(employee);
        }
    }
}

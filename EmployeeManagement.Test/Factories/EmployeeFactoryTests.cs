using EmployeeManagement.Entities;
using EmployeeManagement.Factories;
using Xunit;

namespace EmployeeManagement.Test.Factories
{
    public class EmployeeFactoryTests
    {
        [Fact()]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBe2500()
        {
            var employeeFactory = new EmployeeFactory { };

            var employee = (InternalEmployee)
                employeeFactory.CreateEmployee("FIRSTNAME", "LASTNAME");

            Assert.Equal(2500, employee.Salary);
        }

        [Fact()]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeGreaterThanOrEqualTo2500()
        {
            var employeeFactory = new EmployeeFactory { };

            var employee = (InternalEmployee)
                employeeFactory.CreateEmployee("FIRSTNAME", "LASTNAME");

            Assert.True(employee.Salary >= 2500);
        }

        [Fact()]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500()
        {
            var employeeFactory = new EmployeeFactory { };

            var employee = (InternalEmployee)
                employeeFactory.CreateEmployee("FIRSTNAME", "LASTNAME");

            //Assert.True(employee.Salary >= 2500 && employee.Salary <= 3500,
            //    $"Salary {employee.Salary} is not within an acceptable range,");
            Assert.InRange(employee.Salary, 2500, 3500);
        }

        [Fact()]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500_Precesion()
        {
            var employeeFactory = new EmployeeFactory { };

            var employee = (InternalEmployee)
                employeeFactory.CreateEmployee("FIRSTNAME", "LASTNAME");
            employee.Salary = 2500.123m;

            Assert.Equal(2500, employee.Salary, 0);
        }

        [Fact()]
        public void CreateEmployee_IsExternalIsTrue_ReturnTypeMustBeExternalEmployee()
        {
            // Arrange.
            var employeeFactory = new EmployeeFactory { };

            // Act.
            var employee = employeeFactory.CreateEmployee(
                "FIRSTNAME", "LASTNAME", "COMPANY", true);

            // Assert.
            Assert.IsType<ExternalEmployee>(employee);
            Assert.IsAssignableFrom<Employee>(employee);
        }
    }
}

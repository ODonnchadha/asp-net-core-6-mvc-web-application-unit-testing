using EmployeeManagement.Entities;
using Xunit;

namespace EmployeeManagement.Test.Entities
{
    public class EmployeeTests
    {
        [Fact()]
        public void EmployeeFullNamePropertyGetter_IputFirstNameAndLastName_FullNameIsCorrect()
        {
            var employee = new InternalEmployee(string.Empty, string.Empty, 0, 0, false, 0);

            employee.FirstName = "Flann";
            employee.LastName = "O'Brien";

            Assert.StartsWith(
                employee.FirstName, employee.FullName, StringComparison.InvariantCultureIgnoreCase);
            Assert.Equal("Flann O'Brien", employee.FullName, true, false, false);
        }
    }
}

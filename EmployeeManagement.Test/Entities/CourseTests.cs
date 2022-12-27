using EmployeeManagement.Entities;
using Xunit;

namespace EmployeeManagement.Test.Entities
{
    public class CourseTests
    {
        [Fact()]
        public void CourseConstructor_ConstructCourse_IsNewMustBeTrue()
        {
            var course = new Course("Mining Disasters 101");

            Assert.True(course.IsNew);
        }
    }
}

using EmployeeManagement.Entities;
using Xunit;
using Xunit.Abstractions;

namespace EmployeeManagement.Test.Entities
{
    public class CourseTests
    {
        private readonly ITestOutputHelper helper;
        public CourseTests(ITestOutputHelper helper) => this.helper = helper;
        
        [Fact()]
        public void CourseConstructor_ConstructCourse_IsNewMustBeTrue()
        {
            var course = new Course("Mining Disasters 101");

            helper.WriteLine(course.Title);

            Assert.True(course.IsNew);
        }
    }
}

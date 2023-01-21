using AutoMapper;
using EmployeeManagement.Controllers;
using EmployeeManagement.Entities;
using EmployeeManagement.Interfaces.Services;
using EmployeeManagement.Profiles;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Text;
using Xunit;

namespace EmployeeManagement.Test.Controllers
{
    public class InternalEmployeeControllerTests
    {
        [Fact()]
        public async Task AddInternalEmployee_InvalidInput_MustReturnBadRequast()
        {
            // Arrange.
            var mapper = new Mock<IMapper>();
            var service = new Mock<IEmployeeService>();

            var controller = new InternalEmployeeController(
                service.Object, mapper.Object);

            controller.ModelState.AddModelError("X", "XYZ");

            // Act.
            var result = await controller.AddInternalEmployee(
                new CreateInternalEmployeeViewModel { });

            // Assert.
            var request = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(request.Value);
        }

        [Fact()]
        public async Task InternalEmployeeDetails_InputFromTempData_MustReturnCorrectEmployee()
        {
            // Arrange.
            var id = Guid.NewGuid();

            var service = new Mock<IEmployeeService>();
            service.Setup(
                s => s.FetchInternalEmployeeAsync(It.IsAny<Guid>()))
                .ReturnsAsync(
                new InternalEmployee("FIRSTNAME", "LASTNAME", 3, 4000, true, 1)
                {
                    Id = id,
                    SuggestedBonus= 1,
                });

            var mapper = new Mapper(
                new MapperConfiguration(
                config => config.AddProfile<EmployeeProfile>()));

            var dictionary = new TempDataDictionary(
                new DefaultHttpContext { }, new Mock<ITempDataProvider>().Object);
            dictionary["EmployeeId"] = id;

            var controller = new InternalEmployeeController(
                service.Object, mapper);
            controller.TempData = dictionary;

            // Act.
            var result = await controller.InternalEmployeeDetails(null);

            // Assery.
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<InternalEmployeeDetailViewModel>(view.Model);

            Assert.Equal(id, model.Id);
        }

        [Fact()]
        public async Task InternalEmployeeDetails_InputFromSession_MustReturnCorrectEmployee()
        {
            // Arrange.
            var id = Guid.NewGuid();

            var service = new Mock<IEmployeeService>();
            service.Setup(
                s => s.FetchInternalEmployeeAsync(It.IsAny<Guid>()))
                .ReturnsAsync(
                new InternalEmployee("FIRSTNAME", "LASTNAME", 3, 4000, true, 1)
                {
                    Id = id,
                    SuggestedBonus = 1,
                });

            var mapper = new Mapper(
                new MapperConfiguration(
                config => config.AddProfile<EmployeeProfile>()));

            var session = new Mock<ISession>();
            // NOTE: GetString() is an extension, and extension methods cannot be mocked.
            // session.Setup(s => s.GetString("EmployeeId")).Returns(id.ToString());
            var bytes = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            session.Setup(s => s.TryGetValue(It.IsAny<string>(), out bytes)).Returns(true);

            var context = new DefaultHttpContext { };
            context.Session = session.Object;

            var controller = new InternalEmployeeController(
                service.Object, mapper);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };

            // Act.
            var result = await controller.InternalEmployeeDetails(null);

            // Assery.
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<InternalEmployeeDetailViewModel>(view.Model);

            Assert.Equal(id, model.Id);
        }
    }
}

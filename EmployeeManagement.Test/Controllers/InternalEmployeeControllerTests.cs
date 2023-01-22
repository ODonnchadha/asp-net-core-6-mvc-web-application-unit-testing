using AutoMapper;
using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.Entities;
using EmployeeManagement.Interfaces.Services;
using EmployeeManagement.Profiles;
using EmployeeManagement.Repositories.Tests;
using EmployeeManagement.Services;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace EmployeeManagement.Test.Controllers
{
    public class InternalEmployeeControllerTests
    {
        [Fact()]
        public async Task AddInternalEmployee_InvalidInput_MustReturnBadRequast()
        {
            // Arrange.
            var employee = new Mock<IEmployeeService>();
            var mapper = new Mock<IMapper>();
            var promotion = new Mock<IPromotionService>();

            var controller = new InternalEmployeeController(
                employee.Object, mapper.Object, promotion.Object);

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

            var employee = new Mock<IEmployeeService>();
            employee.Setup(
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

            var promotion = new Mock<IPromotionService>();

            var controller = new InternalEmployeeController(
                employee.Object, mapper, promotion.Object);
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

            var employee = new Mock<IEmployeeService>();
            employee.Setup(
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

            var promotion = new Mock<IPromotionService>();

            var controller = new InternalEmployeeController(
                employee.Object, mapper, promotion.Object);

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

        [Fact()]
        public async Task ExecutePromotionRequest_RequestPromotion_MustPromoteEmployee()
        {
            // Arrange.
            var id = Guid.NewGuid();
            var level = 1;

            var employee = new Mock<IEmployeeService>();
            employee.Setup(
                s => s.FetchInternalEmployeeAsync(It.IsAny<Guid>()))
                .ReturnsAsync(
                new InternalEmployee("FIRSTNAME", "LASTNAME", 3, 4000, true, level)
                {
                    Id = id,
                    SuggestedBonus = 1,
                });

            var mapper = new Mapper(
                new MapperConfiguration(
                config => config.AddProfile<EmployeeProfile>()));

            var handler = new Mock<HttpMessageHandler>();
            handler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        JsonSerializer.Serialize(
                            new PromotionEligibility
                            { 
                                EligibleForPromotion = true 
                            },
                            new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                            }), 
                        Encoding.ASCII, 
                        "application/json")
                });

            var client = new HttpClient(handler.Object);

            // var promotion = new Mock<IPromotionService>();
            var promotion = new PromotionService(
                client, new EmployeeManagementTestDataRepository { });

            var controller = new InternalEmployeeController(
                employee.Object, mapper, promotion);

            // Act.
            var result = await controller.ExecutePromotionRequest(id);

            // Assery.
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<InternalEmployeeDetailViewModel>(view.Model);

            Assert.Equal(id, model.Id);
            Assert.Equal(++level, model.JobLevel);
        }
    }
}

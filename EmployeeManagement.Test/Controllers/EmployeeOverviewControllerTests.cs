using AutoMapper;
using EmployeeManagement.Controllers;
using EmployeeManagement.Entities;
using EmployeeManagement.Interfaces.Services;
using EmployeeManagement.Profiles;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace EmployeeManagement.Test.Controllers
{
    public class EmployeeOverviewControllerTests
    {
        private EmployeeOverviewController controller;
        private InternalEmployee employee;
        public EmployeeOverviewControllerTests()
        {
            this.employee = new InternalEmployee(
                "FIRSTNAME1", "LASTNAME1", 2, 3000, false, 2)
            {
                Id = Guid.Parse("72f2f5fe-e50c-4966-8420-d50258aefdcb"),
                SuggestedBonus= 400
            };

            var service = new Mock<IEmployeeService>();
            service.Setup(s => s.FetchInternalEmployeesAsync()).ReturnsAsync(
                new List<InternalEmployee>
                {
                    this.employee,
                    new InternalEmployee("FIRSTNAME2", "LASTNAME2", 3, 3400, true, 1),
                    new InternalEmployee("FIRSTNAME3", "LASTNAME3", 3, 4000, false, 3)
                });

            //var mapper = new Mock<IMapper>();
            //mapper.Setup(
            //    m => m.Map<InternalEmployee, InternalEmployeeForOverviewViewModel>
            //    (It.IsAny<InternalEmployee>())).Returns(
            //    new InternalEmployeeForOverviewViewModel());

            var configuration = new MapperConfiguration(config =>
                config.AddProfile<EmployeeProfile>());
            var mapper = new Mapper(configuration);

            this.controller = new EmployeeOverviewController(service.Object, mapper);
        }

        [Fact()]
        public async Task Index_GetAction_MustReturnViewResult()
        {
            // Act.
            var result = await controller.Index();

            // Assert.
            Assert.IsType<ViewResult>(result);
        }

        [Fact()]
        public async Task Index_GetAction_MustReturnEmployeeOverviewViewModelAsViewModelType()
        {
            // Act.
            var result = await controller.Index();

            // Assert.
            var view = Assert.IsType<ViewResult>(result);
            Assert.IsType<EmployeeOverviewViewModel>(view.Model);

            Assert.IsType<EmployeeOverviewViewModel>(((ViewResult)result).Model);
        }

        [Fact(Skip = "e.g.:")]
        public async Task Index_GetAction_MustReturnNumberOfInputtedInternalEmployees()
        {
            // Act.
            var result = await controller.Index();

           // Assert.
           //Assert.Equal(3, ((EmployeeOverviewViewModel)(
           //    (ViewResult)result).Model).InternalEmployees.Count);
        }

        [Fact()]
        public async Task Index_GetAction_ReturnsViewREsultWithInternalEmployees()
        {
            // Act.
            var result = await controller.Index();

            // Assert.
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<EmployeeOverviewViewModel>(view.Model);

            Assert.Equal(3, model.InternalEmployees.Count);

            var first = model.InternalEmployees.First();
            Assert.Equal(first.Id, employee.Id);
            Assert.Equal(first.LastName, employee.LastName);
            Assert.Equal(first.YearsInService, employee.YearsInService);
            Assert.Equal(first.SuggestedBonus, employee.SuggestedBonus);
        }

        /// <summary>
        /// Concrete objects.
        /// </summary>
        [Fact()]
        public void ProtectedIndex_GetActionForUserInAdminRole_ReturnsDirectToAdminIndex()
        {
            // Arrange.
            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Flann O'Brien"),
                new Claim(ClaimTypes.Role, "Admin"),
            }, "X");

            var context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(identity)
            };

            controller.ControllerContext =
                new ControllerContext { HttpContext = context };

            // Act.
            var result = controller.ProtectedIndex();

            // Assert.
            var action = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("AdminIndex", action.ActionName);
            Assert.Equal("EmployeeManagement", action.ControllerName);
        }

        /// <summary>
        /// Mocks.
        /// </summary>
        [Fact()]
        public void ProtectedIndex_GetActionForUserInAdminRoleMocks_ReturnsDirectToAdminIndex()
        {
            // Arrange.
            var principle = new Mock<ClaimsPrincipal>();
            principle.Setup(
                p => p.IsInRole(It.Is<string>(
                    str => str == "Admin"))).Returns(true);
                
            var context = new Mock<HttpContext>();
            context.Setup(c => c.User).Returns(principle.Object);

            controller.ControllerContext =
                new ControllerContext { HttpContext = context.Object };

            // Act.
            var result = controller.ProtectedIndex();

            // Assert.
            var action = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("AdminIndex", action.ActionName);
            Assert.Equal("EmployeeManagement", action.ControllerName);
        }
    }
}

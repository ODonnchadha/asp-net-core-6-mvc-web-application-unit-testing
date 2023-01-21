using AutoMapper;
using EmployeeManagement.Controllers;
using EmployeeManagement.Profiles;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace EmployeeManagement.Test.Controllers
{
    public class StatisticsControllerTests
    {
        [Fact()]
        public void Index_InputFromHttpConnectionFeature_MustReturnInputtedIps()
        {
            // Arrange.
            var ip1 = "1.1.1.1";
            var port1 = 1010;
            var ip2 = "2.2.2.2";
            var port2 = 2020;

            var features = new Mock<IFeatureCollection>();
            features.Setup(f => f.Get<IHttpConnectionFeature>())
                .Returns(new HttpConnectionFeature
                {
                    LocalIpAddress = IPAddress.Parse(ip1),
                    LocalPort = port1,
                    RemoteIpAddress = IPAddress.Parse(ip2),
                    RemotePort = port2
                });

            var context = new Mock<HttpContext>();
            context.Setup(c => c.Features).Returns(features.Object);

            var controller = new StatisticsController(
                new Mapper(new MapperConfiguration(
                c => c.AddProfile<StatisticsProfile>())));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context.Object
            };

            // Act.
            var result = controller.Index();

            // Assert.
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<StatisticsViewModel>(view.Model);

            Assert.Equal(ip1, model.LocalIpAddress);
            Assert.Equal(port1, model.LocalPort);
            Assert.Equal(ip2, model.RemoteIpAddress);
            Assert.Equal(port2, model.RemotePort);
        }
    }
}

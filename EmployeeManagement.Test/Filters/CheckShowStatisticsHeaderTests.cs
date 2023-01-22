using EmployeeManagement.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Xunit;

namespace EmployeeManagement.Test.Filters
{
    public class CheckShowStatisticsHeaderTests
    {
        [Fact()]
        public void OnActionExecuting_InvokeWithoutShowStatisticsHeader_ReturnsBadRequest()
        {
            // Arrange.
            var filter = new CheckShowStatisticsHeader { };

            var action = new ActionContext(
                new DefaultHttpContext { }, new(), new(), new());

            var executing = new ActionExecutingContext(
                action,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>(),
                controller: null);

            // Act.
            filter.OnActionExecuting(executing);

            // Assert.
            Assert.IsType<BadRequestResult>(executing.Result);
        }
    }
}

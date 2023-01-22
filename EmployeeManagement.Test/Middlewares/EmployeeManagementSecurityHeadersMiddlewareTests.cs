using EmployeeManagement.Middleware;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace EmployeeManagement.Test.Middlewares
{
    public class EmployeeManagementSecurityHeadersMiddlewareTests
    {
        [Fact()]
        public async Task InvokeAsync_Invoke_SetsExpectedResponseHeaders()
        {
            // Arrange.
            var context = new DefaultHttpContext { };
            RequestDelegate next = (HttpContext context) => Task.CompletedTask;

            var middleware = new EmployeeManagementSecurityHeadersMiddleware(next);

            // Act.
            await middleware.InvokeAsync(context);

            // Assert.
            var policy = context.Response.Headers["Content-Security-Policy"].ToString();
            var options = context.Response.Headers["X-Content-Type-Options"].ToString();

            Assert.Equal("default-src 'self';frame-ancestors 'none';", policy);
            Assert.Equal("nosniff", options);
        }
    }
}

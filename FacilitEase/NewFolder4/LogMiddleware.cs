using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging; // Add this for ILogger
using System.Threading.Tasks;

namespace FacilitEase.NewFolder4
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogMiddleware> _logger; 

        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            _logger.LogInformation($"Handling request for {httpContext.Request.Path}");

            await _next(httpContext);
            _logger.LogInformation($"Completed request for {httpContext.Request.Path}");
        }
    }

    public static class LogMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogMiddleware>();
        }
    }
}

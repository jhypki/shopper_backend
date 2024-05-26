using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace NetCoreWebAPI.Middleware
{
    // Step1 
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exc)
            {
                var appTrackId = Guid.NewGuid().ToString();
                var problemDetail = new ProblemDetails()
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Title = "Internal server error",
                    Detail = $"An error occured while proccessing the request. Application traking Id :{appTrackId}",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Instance = context.Request.Path,

                };
                _logger.LogError($"An error occured, StackTrace:{exc.StackTrace}, Message: {exc.Message}, Application Tracking Id : {appTrackId}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(problemDetail);
            }
        }
    }
}
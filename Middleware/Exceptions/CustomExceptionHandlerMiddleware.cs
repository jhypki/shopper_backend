using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ShopperBackend.Exceptions
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
                // {
                //     await HandleExceptionAsync(context, new CustomNotFoundException("Resource not found"));
                // }
                // else if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest)
                // {
                //     await HandleExceptionAsync(context, new CustomBadRequestException("Bad request"));
                // }
                switch (context.Response.StatusCode)
                {
                    case (int)HttpStatusCode.NotFound:
                        await HandleExceptionAsync(context, new CustomNotFoundException("Resource not found"));
                        break;
                    case (int)HttpStatusCode.BadRequest:
                        await HandleExceptionAsync(context, new CustomBadRequestException("Bad request"));
                        break;
                    case (int)HttpStatusCode.Unauthorized:
                        await HandleExceptionAsync(context, new CustomUnauthorizedException("Unauthorized"));
                        break;
                    case (int)HttpStatusCode.Forbidden:
                        await HandleExceptionAsync(context, new CustomForbiddenException("Forbidden"));
                        break;
                    case (int)HttpStatusCode.Conflict:
                        await HandleExceptionAsync(context, new CustomConflictException("Conflict"));
                        break;
                    case (int)HttpStatusCode.InternalServerError:
                        await HandleExceptionAsync(context, new CustomInternalServerErrorException("Internal server error"));
                        break;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("The response has already started, the error handler will not be executed.");
                return;
            }

            context.Response.Clear();
            context.Response.ContentType = "application/json";

            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "";

            switch (exception)
            {
                case CustomNotFoundException _:
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;
                case CustomBadRequestException _:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;
                case CustomUnauthorizedException _:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    message = exception.Message;
                    break;
                case CustomForbiddenException _:
                    statusCode = (int)HttpStatusCode.Forbidden;
                    message = exception.Message;
                    break;
                case CustomConflictException _:
                    statusCode = (int)HttpStatusCode.Conflict;
                    message = exception.Message;
                    break;
                case CustomInternalServerErrorException _:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = exception.Message;
                    break;
                default:
                    _logger.LogError(exception, "An unexpected error occurred.");
                    message = "An unexpected error occurred. Please try again later.";
                    break;
            }

            context.Response.StatusCode = statusCode;

            var result = new
            {
                statusCode,
                message,
                details = exception.InnerException?.Message
            };

            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
        }
    }
}

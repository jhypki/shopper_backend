namespace ShopperBackend.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly string _id;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger, string id)
        {
            _next = next;
            _logger = logger;
            _id = id;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log the request details with ID
            _logger.LogInformation($"Middleware {_id} - Incoming request: {context.Request.Method} {context.Request.Path}");

            // Call the next middleware in the pipeline
            await _next(context);

            // Log the response details with ID
            _logger.LogInformation($"Middleware {_id} - Outgoing response: {context.Response.StatusCode}");

        }
    }
}

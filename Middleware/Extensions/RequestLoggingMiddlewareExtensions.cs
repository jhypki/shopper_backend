namespace ShopperBackend.Middlewares;

public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLoggingMiddleware(this IApplicationBuilder builder, string id)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>(id);
    }
}

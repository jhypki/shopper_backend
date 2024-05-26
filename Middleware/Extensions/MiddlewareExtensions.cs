using Microsoft.AspNetCore.Builder;

namespace ShopperBackend.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomErrorHandlingMiddleware>();
        }
    }
}

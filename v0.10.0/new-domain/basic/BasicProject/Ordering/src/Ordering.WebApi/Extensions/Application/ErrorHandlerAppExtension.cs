namespace Ordering.WebApi.Extensions.Application
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Ordering.WebApi.Middleware;

    public static class ErrorHandlerAppExtension
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
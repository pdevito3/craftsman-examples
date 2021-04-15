namespace Billing.WebApi.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Billing.WebApi.Middleware;

    public static class AppExtensions
    {
        // Swagger Marker - Do Not Delete
        public static void UseSwaggerExtension(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                config.OAuthClientId(configuration["JwtSettings:ClientId"]);
                config.OAuthUsePkce();
            });
        }

        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}

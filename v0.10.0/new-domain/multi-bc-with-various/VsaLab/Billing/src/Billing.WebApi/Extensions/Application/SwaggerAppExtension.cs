namespace Billing.WebApi.Extensions.Application
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Billing.WebApi.Middleware;

    public static class SwaggerAppExtension
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }
    }
}
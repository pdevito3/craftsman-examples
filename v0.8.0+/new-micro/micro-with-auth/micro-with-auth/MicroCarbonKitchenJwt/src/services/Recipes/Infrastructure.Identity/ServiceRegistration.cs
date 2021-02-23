namespace Infrastructure.Identity
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceRegistration
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            if(env.EnvironmentName != "IntegrationTesting")
            {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = configuration["JwtSettings:Authority"];
                        options.Audience = configuration["JwtSettings:Audience"];
                    });
            }

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanReadRecipes", 
                    policy => policy.RequireClaim("scope", "recipes.read"));
                options.AddPolicy("CanAddRecipes", 
                    policy => policy.RequireClaim("scope", "recipes.add"));
                options.AddPolicy("CanDeleteRecipes", 
                    policy => policy.RequireClaim("scope", "recipes.delete"));
                options.AddPolicy("CanUpdateRecipes", 
                    policy => policy.RequireClaim("scope", "recipes.update"));
            });
        }
    }
}

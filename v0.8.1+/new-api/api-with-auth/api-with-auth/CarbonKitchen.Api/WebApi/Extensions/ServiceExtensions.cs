namespace WebApi.Extensions
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using System;
    using System.Collections.Generic;

    public static class ServiceExtensions
    {
        #region Swagger Region - Do Not Delete
            public static void AddSwaggerExtension(this IServiceCollection services, IConfiguration configuration)
            {
                services.AddSwaggerGen(config =>
                {
                    config.SwaggerDoc(
                        "v1", 
                        new OpenApiInfo
                        {
                            Version = "v1",
                            Title = "MySwaggerDoc",
                            Description = "This is my swagger doc",
                            Contact = new OpenApiContact
                            {
                                Name = "Paul",
                                Email = "paul@test.com",
                                Url = new Uri("https://www.thisispaulswebsite.com"),
                            },
                        });

                    config.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri(configuration["JwtSettings:AuthorizationUrl"]),
                                TokenUrl = new Uri(configuration["JwtSettings:TokenUrl"]),
                                Scopes = new Dictionary<string, string>
                                {
                                    { "recipes.read","CanReadRecipes" },

                                    { "recipes.add","CanAddRecipes" },

                                    { "recipes.delete","CanDeleteRecipes" },

                                    { "recipes.update","CanUpdateRecipes" },

                                }
                            }
                        }
                    });

                    config.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "oauth2"
                                },
                                Scheme = "oauth2",
                                Name = "oauth2",
                                In = ParameterLocation.Header
                            },
                            new List<string>()
                        }
                    }); 

                    config.IncludeXmlComments(string.Format(@"{0}\CarbonKitchen.Api.WebApi.xml", AppDomain.CurrentDomain.BaseDirectory));
                });
            }
        #endregion

        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                // Default API Version
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // use default version when version is not specified
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });
        }

        public static void AddCorsService(this IServiceCollection services, string policyName)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(policyName,
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Pagination"));
            });
        }
    }
}

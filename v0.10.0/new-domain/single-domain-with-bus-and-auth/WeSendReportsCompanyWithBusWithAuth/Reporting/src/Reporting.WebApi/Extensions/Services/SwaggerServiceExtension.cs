namespace Reporting.WebApi.Extensions.Services
{
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Reflection;

    public static class SwaggerServiceExtension
    {
        public static void AddSwaggerExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Reporting Swagger Doc",
                        Description = "Swagger doc for reporting",
                        Contact = new OpenApiContact
                        {
                            Name = "",
                            Email = "",
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
                                    { "reportrequest.delete","CanDeleteReportRequest" },
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

                config.IncludeXmlComments(string.Format(@$"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}Reporting.WebApi.xml"));
            });
        }
    }
}

namespace Lab.Api.Tests
{
    using Infrastructure.Persistence.Contexts;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Respawn;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using WebApi;

    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        // checkpoint for respawn to clear the database when spinning up each time
        private static Checkpoint checkpoint = new Checkpoint
        {
            
        };

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("IntegrationTesting");

            builder.ConfigureServices(async services =>
            {
                // Create a new service provider.
                var provider = services.BuildServiceProvider();

                // Add a database context (LabDbContext) using an in-memory 
                // database for testing.
                services.AddDbContext<LabDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<LabDbContext>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();

                    try
                    {
                        await checkpoint.Reset(db.Database.GetDbConnection());
                    }
                    catch
                    {
                    }
                }
            });
        }

        public HttpClient GetAnonymousClient()
        {
            return CreateClient();
        }
    }
}
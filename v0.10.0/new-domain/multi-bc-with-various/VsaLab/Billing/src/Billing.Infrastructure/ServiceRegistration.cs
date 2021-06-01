namespace Billing.Infrastructure
{
    using Billing.Infrastructure.Contexts;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Sieve.Services;

    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            // DbContext -- Do Not Delete
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<BillingDbContext>(options =>
                    options.UseInMemoryDatabase($"BillingDbContext"));
            }
            else
            {
                services.AddDbContext<BillingDbContext>(options =>
                    options.UseNpgsql(
                        configuration.GetConnectionString("BillingDbContext"),
                        builder => builder.MigrationsAssembly(typeof(BillingDbContext).Assembly.FullName)));
            }

            services.AddScoped<SieveProcessor>();

            // Auth -- Do Not Delete
        }
    }
}

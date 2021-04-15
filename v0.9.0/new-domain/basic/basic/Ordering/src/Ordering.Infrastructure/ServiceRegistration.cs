namespace Ordering.Infrastructure
{
    using Ordering.Infrastructure.Contexts;
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
                services.AddDbContext<OrderingDbContext>(options =>
                    options.UseInMemoryDatabase($"OrderingDbContext"));
            }
            else
            {
                services.AddDbContext<OrderingDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("OrderingDbContext"),
                        builder => builder.MigrationsAssembly(typeof(OrderingDbContext).Assembly.FullName)));
            }

            services.AddScoped<SieveProcessor>();

            // Auth -- Do Not Delete
        }
    }
}

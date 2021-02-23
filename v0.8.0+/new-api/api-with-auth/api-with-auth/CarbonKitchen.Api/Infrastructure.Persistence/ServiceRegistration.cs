namespace Infrastructure.Persistence
{
    using Infrastructure.Persistence.Contexts;
    using Application.Interfaces.Recipe;
    using Infrastructure.Persistence.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Sieve.Services;

    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext -- Do Not Delete            
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<CarbonKitchenDbContext>(options =>
                    options.UseInMemoryDatabase($"CarbonKitchenDbContext"));
            }
            else
            {
                services.AddDbContext<CarbonKitchenDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("CarbonKitchenDbContext"),
                        builder => builder.MigrationsAssembly(typeof(CarbonKitchenDbContext).Assembly.FullName)));
            }
            #endregion

            services.AddScoped<SieveProcessor>();

            #region Repositories -- Do Not Delete
            services.AddScoped<IRecipeRepository, RecipeRepository>();
            #endregion
        }
    }
}

namespace Infrastructure.Persistence
{
    using Infrastructure.Persistence.Contexts;
    using Application.Interfaces.Patient;
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
                services.AddDbContext<LabDbContext>(options =>
                    options.UseInMemoryDatabase($"LabDbContext"));
            }
            else
            {
                services.AddDbContext<LabDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("LabDbContext"),
                        builder => builder.MigrationsAssembly(typeof(LabDbContext).Assembly.FullName)));
            }
            #endregion

            services.AddScoped<SieveProcessor>();

            #region Repositories -- Do Not Delete
            services.AddScoped<IPatientRepository, PatientRepository>();
            #endregion
        }
    }
}

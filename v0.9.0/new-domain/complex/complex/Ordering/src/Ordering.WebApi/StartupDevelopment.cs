namespace Ordering.WebApi
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Ordering.Infrastructure;
    using Ordering.Infrastructure.Seeders;
    using Ordering.Infrastructure.Contexts;
    using Ordering.WebApi.Extensions;
    using Serilog;

    public class StartupDevelopment
    {
        public IConfiguration _config { get; }
        public IWebHostEnvironment _env { get; }

        public StartupDevelopment(IConfiguration configuration, IWebHostEnvironment env)
        {
            _config = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsService("OrderingCorsPolicy");
            services.AddInfrastructure(_config, _env);
            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddApiVersioningExtension();
            services.AddWebApiServices();
            services.AddHealthChecks();

            // Dynamic Services
            services.AddSwaggerExtension(_config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            #region Entity Context Region - Do Not Delete

                using (var context = app.ApplicationServices.GetService<OrderingDbContext>())
                {
                    context.Database.EnsureCreated();

                    #region OrderingDbContext Seeder Region - Do Not Delete
                    
                    PatientSeeder.SeedSamplePatientData(app.ApplicationServices.GetService<OrderingDbContext>());
                    SampleSeeder.SeedSampleSampleData(app.ApplicationServices.GetService<OrderingDbContext>());
                    #endregion
                }

            #endregion

            app.UseCors("OrderingCorsPolicy");

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseErrorHandlingMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/api/health");
                endpoints.MapControllers();
            });

            // Dynamic App
            app.UseSwaggerExtension(_config);
        }
    }
}

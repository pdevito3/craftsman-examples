namespace Ordering.WebApi.Extensions.Services
{
    using MassTransit;
    using Ordering.WebApi.Extensions.Services.ProducerRegistrations;
    using Messages;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using RabbitMQ.Client;
    using System.Reflection;

    public static class MassTransitServiceExtension
    {
        public static void AddMassTransitServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (!configuration.GetValue<bool>("UseInMemoryBus"))
            {
                services.AddMassTransit(mt =>
                {
                    mt.AddConsumers(Assembly.GetExecutingAssembly());
                    mt.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(configuration["RMQ:Host"], configuration["RMQ:VirtualHost"], h =>
                        {
                            h.Username(configuration["RMQ:Username"]);
                            h.Password(configuration["RMQ:Password"]);
                        });

                        // Producers -- Do Not Delete This Comment
                        cfg.PatientUpdatedEndpoint();

                        // Consumers -- Do Not Delete This Comment
                    });
                });
                services.AddMassTransitHostedService();
            }
        }
    }
}

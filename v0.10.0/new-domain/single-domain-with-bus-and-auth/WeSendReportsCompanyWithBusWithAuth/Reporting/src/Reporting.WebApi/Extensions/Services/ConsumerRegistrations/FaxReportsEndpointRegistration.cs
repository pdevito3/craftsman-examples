namespace Reporting.WebApi.Extensions.Services.ConsumerRegistrations
{
    using MassTransit;
    using MassTransit.RabbitMqTransport;
    using RabbitMQ.Client;
    using Reporting.WebApi.Features.Consumers;

    public static class FaxReportsEndpointRegistration
    {
        public static void FaxReportsEndpoint(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
        {
            cfg.ReceiveEndpoint("fax-reports", re =>
            {
                // turns off default fanout settings
                re.ConfigureConsumeTopology = false;

                // the consumers that are subscribed to the endpoint
                re.ConfigureConsumer<SendRequestedFax>(context);

                // the binding of the intermediary exchange and the primary exchange
                re.Bind("report-requests", e =>
                {
                    e.RoutingKey = "fax";
                    e.ExchangeType = ExchangeType.Direct;
                });
            });
        }
    }
}
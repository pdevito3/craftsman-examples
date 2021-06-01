namespace Reporting.WebApi.Extensions.Services.ConsumerRegistrations
{
    using MassTransit;
    using MassTransit.RabbitMqTransport;
    using RabbitMQ.Client;
    using Reporting.WebApi.Features.Consumers;

    public static class EmailReportsEndpointRegistration
    {
        public static void EmailReportsEndpoint(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
        {
            cfg.ReceiveEndpoint("email-reports", re =>
            {
                // turns off default fanout settings
                re.ConfigureConsumeTopology = false;

                // a replicated queue to provide high availability and data safety. available in RMQ 3.8+
                re.SetQuorumQueue();

                // enables a lazy queue for more stable cluster with better predictive performance.
                // Please note that you should disable lazy queues if you require really high performance, if the queues are always short, or if you have set a max-length policy.
                re.SetQueueArgument("declare", "lazy");

                // the consumers that are subscribed to the endpoint
                re.ConfigureConsumer<SendRequestedEmail>(context);

                // the binding of the intermediary exchange and the primary exchange
                re.Bind("report-requests", e =>
                {
                    e.RoutingKey = "email";
                    e.ExchangeType = ExchangeType.Direct;
                });
            });
        }
    }
}
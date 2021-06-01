namespace Reporting.WebApi.Extensions.Services.ProducerRegistrations
{
    using MassTransit;
    using MassTransit.RabbitMqTransport;
    using Messages;
    using RabbitMQ.Client;

    public static class EmailRequestorRegistration
    {
        public static void EmailRequestor(this IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.Message<ISendReportRequest>(e => e.SetEntityName("report-requests")); // name of the primary exchange
            cfg.Publish<ISendReportRequest>(e => e.ExchangeType = ExchangeType.Direct); // primary exchange type

            // configuration for the exchange and routing key
            cfg.Send<ISendReportRequest>(e =>
            {
                // **Use the `UseRoutingKeyFormatter` to configure what to use for the routing key when sending a message of type `ISendReportRequest`**
                /* Examples
                *
                * Direct example: uses the `ProductType` message property as a key
                * e.UseRoutingKeyFormatter(context => context.Message.ProductType.ToString());
                *
                * Topic example: uses the VIP Status and ClientType message properties to make a key.
                * e.UseRoutingKeyFormatter(context =>
                * {
                *     var vipStatus = context.Message.IsVip ? "vip" : "normal";
                *     return $"{vipStatus}.{context.Message.ClientType}";
                * });
                */
            });
        }
    }
}
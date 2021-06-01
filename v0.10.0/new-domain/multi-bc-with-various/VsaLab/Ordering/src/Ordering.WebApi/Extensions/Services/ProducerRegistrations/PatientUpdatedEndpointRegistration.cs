namespace Ordering.WebApi.Extensions.Services.ProducerRegistrations
{
    using MassTransit;
    using MassTransit.RabbitMqTransport;
    using Messages;
    using RabbitMQ.Client;

    public static class PatientUpdatedEndpointRegistration
    {
        public static void PatientUpdatedEndpoint(this IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.Message<IPatientUpdated>(e => e.SetEntityName("patient-updated")); // name of the primary exchange
            cfg.Publish<IPatientUpdated>(e => e.ExchangeType = ExchangeType.Fanout); // primary exchange type
        }
    }
}
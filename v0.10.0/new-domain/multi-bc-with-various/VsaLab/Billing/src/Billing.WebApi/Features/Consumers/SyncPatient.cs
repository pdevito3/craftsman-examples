namespace Billing.WebApi.Features.Consumers
{
    using AutoMapper;
    using MassTransit;
    using Messages;
    using System.Threading.Tasks;
    using Billing.Infrastructure.Contexts;

    public class SyncPatient : IConsumer<IPatientUpdated>
    {
        private readonly IMapper _mapper;
        private readonly BillingDbContext _db;

        public SyncPatient(BillingDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public class SyncPatientProfile : Profile
        {
            public SyncPatientProfile()
            {
                //createmap<to this, from this>
            }
        }

        public Task Consume(ConsumeContext<IPatientUpdated> context)
        {
            // do work here

            return Task.CompletedTask;
        }
    }
}
namespace Reporting.WebApi.Features.Consumers
{
    using AutoMapper;
    using MassTransit;
    using Messages;
    using System.Threading.Tasks;
    using Reporting.Infrastructure.Contexts;

    public class SendRequestedEmail : IConsumer<ISendReportRequest>
    {
        private readonly IMapper _mapper;
        private readonly ReportingDbContext _db;

        public SendRequestedEmail(ReportingDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public class SendRequestedEmailProfile : Profile
        {
            public SendRequestedEmailProfile()
            {
                //createmap<to this, from this>
            }
        }

        public Task Consume(ConsumeContext<ISendReportRequest> context)
        {
            // do work here

            return Task.CompletedTask;
        }
    }
}
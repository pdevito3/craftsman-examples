namespace Reporting.WebApi.Features.Consumers
{
    using AutoMapper;
    using MassTransit;
    using Messages;
    using System.Threading.Tasks;

    public class SendRequestedFax : IConsumer<ISendReportRequest>
    {
        private readonly IMapper _mapper;

        public SendRequestedFax(IMapper mapper)
        {
            _mapper = mapper;
        }

        public class SendRequestedFaxProfile : Profile
        {
            public SendRequestedFaxProfile()
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
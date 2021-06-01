namespace Reporting.WebApi.Features.Producers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MassTransit;
    using Messages;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Reporting.Infrastructure.Contexts;

    public static class EmailWasRequested
    {
        public class EmailWasRequestedCommand : IRequest<bool>
        {
            public string SomeContentToPutInMessage { get; set; }

            public EmailWasRequestedCommand(string someContentToPutInMessage)
            {
                SomeContentToPutInMessage = someContentToPutInMessage;
            }
        }

        public class Handler : IRequestHandler<EmailWasRequestedCommand, bool>
        {
            private readonly IPublishEndpoint _publishEndpoint;
            private readonly IMapper _mapper;
            private readonly ReportingDbContext _db;

            public Handler(ReportingDbContext db, IMapper mapper, IPublishEndpoint publishEndpoint)
            {
                _publishEndpoint = publishEndpoint;
                _mapper = mapper;
                _db = db;
            }

            public class EmailWasRequestedCommandProfile : Profile
            {
                public EmailWasRequestedCommandProfile()
                {
                    //createmap<to this, from this>
                }
            }

            public async Task<bool> Handle(EmailWasRequestedCommand request, CancellationToken cancellationToken)
            {
                var message = new
                {
                    // map content to message here or with automapper
                };
                await _publishEndpoint.Publish<ISendReportRequest>(message);

                return true;
            }
        }
    }
}
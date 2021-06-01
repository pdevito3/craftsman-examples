namespace Ordering.WebApi.Features.Producers
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
    using Ordering.Infrastructure.Contexts;

    public static class PatientUpdated
    {
        public class PatientUpdatedCommand : IRequest<bool>
        {
            public string SomeContentToPutInMessage { get; set; }

            public PatientUpdatedCommand(string someContentToPutInMessage)
            {
                SomeContentToPutInMessage = someContentToPutInMessage;
            }
        }

        public class Handler : IRequestHandler<PatientUpdatedCommand, bool>
        {
            private readonly IPublishEndpoint _publishEndpoint;
            private readonly IMapper _mapper;
            private readonly OrderingDbContext _db;

            public Handler(OrderingDbContext db, IMapper mapper, IPublishEndpoint publishEndpoint)
            {
                _publishEndpoint = publishEndpoint;
                _mapper = mapper;
                _db = db;
            }

            public class PatientUpdatedCommandProfile : Profile
            {
                public PatientUpdatedCommandProfile()
                {
                    //createmap<to this, from this>
                }
            }

            public async Task<bool> Handle(PatientUpdatedCommand request, CancellationToken cancellationToken)
            {
                var message = new
                {
                    // map content to message here or with automapper
                };
                await _publishEndpoint.Publish<IPatientUpdated>(message);

                return true;
            }
        }
    }
}
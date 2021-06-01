namespace Ordering.WebApi.Features.Samples
{
    using Ordering.Core.Dtos.Sample;
    using Ordering.Core.Exceptions;
    using Ordering.Infrastructure.Contexts;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public static class GetSample
    {
        public class SampleQuery : IRequest<SampleDto>
        {
            public Guid SampleId { get; set; }

            public SampleQuery(Guid sampleId)
            {
                SampleId = sampleId;
            }
        }

        public class Handler : IRequestHandler<SampleQuery, SampleDto>
        {
            private readonly OrderingDbContext _db;
            private readonly IMapper _mapper;

            public Handler(OrderingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<SampleDto> Handle(SampleQuery request, CancellationToken cancellationToken)
            {
                // add logger (and a try catch with logger so i can cap the unexpected info)........ unless this happens in my logger decorator that i am going to add?

                var result = await _db.Samples
                    .ProjectTo<SampleDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(s => s.SampleId == request.SampleId);

                if (result == null)
                {
                    // log error
                    throw new KeyNotFoundException();
                }

                return result;
            }
        }
    }
}
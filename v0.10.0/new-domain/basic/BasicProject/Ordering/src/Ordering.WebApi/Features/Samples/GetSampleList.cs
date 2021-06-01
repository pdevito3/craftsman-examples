namespace Ordering.WebApi.Features.Samples
{
    using Ordering.Core.Entities;
    using Ordering.Core.Dtos.Sample;
    using Ordering.Core.Exceptions;
    using Ordering.Infrastructure.Contexts;
    using Ordering.Core.Wrappers;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Sieve.Models;
    using Sieve.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public static class GetSampleList
    {
        public class SampleListQuery : IRequest<PagedList<SampleDto>>
        {
            public SampleParametersDto QueryParameters { get; set; }

            public SampleListQuery(SampleParametersDto queryParameters)
            {
                QueryParameters = queryParameters;
            }
        }

        public class Handler : IRequestHandler<SampleListQuery, PagedList<SampleDto>>
        {
            private readonly OrderingDbContext _db;
            private readonly SieveProcessor _sieveProcessor;
            private readonly IMapper _mapper;

            public Handler(OrderingDbContext db, IMapper mapper, SieveProcessor sieveProcessor)
            {
                _mapper = mapper;
                _db = db;
                _sieveProcessor = sieveProcessor;
            }

            public async Task<PagedList<SampleDto>> Handle(SampleListQuery request, CancellationToken cancellationToken)
            {
                if (request.QueryParameters == null)
                {
                    // log error
                    throw new ApiException("Invalid query parameters.");
                }

                var collection = _db.Samples
                    as IQueryable<Sample>;

                var sieveModel = new SieveModel
                {
                    Sorts = request.QueryParameters.SortOrder ?? "SampleId",
                    Filters = request.QueryParameters.Filters
                };

                var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
                var dtoCollection = appliedCollection
                    .ProjectTo<SampleDto>(_mapper.ConfigurationProvider);

                return await PagedList<SampleDto>.CreateAsync(dtoCollection,
                    request.QueryParameters.PageNumber,
                    request.QueryParameters.PageSize);
            }
        }
    }
}
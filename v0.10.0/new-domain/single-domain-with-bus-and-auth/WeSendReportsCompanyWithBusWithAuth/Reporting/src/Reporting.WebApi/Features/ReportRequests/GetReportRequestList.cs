namespace Reporting.WebApi.Features.ReportRequests
{
    using Reporting.Core.Entities;
    using Reporting.Core.Dtos.ReportRequest;
    using Reporting.Core.Exceptions;
    using Reporting.Infrastructure.Contexts;
    using Reporting.Core.Wrappers;
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

    public static class GetReportRequestList
    {
        public class ReportRequestListQuery : IRequest<PagedList<ReportRequestDto>>
        {
            public ReportRequestParametersDto QueryParameters { get; set; }

            public ReportRequestListQuery(ReportRequestParametersDto queryParameters)
            {
                QueryParameters = queryParameters;
            }
        }

        public class Handler : IRequestHandler<ReportRequestListQuery, PagedList<ReportRequestDto>>
        {
            private readonly ReportingDbContext _db;
            private readonly SieveProcessor _sieveProcessor;
            private readonly IMapper _mapper;

            public Handler(ReportingDbContext db, IMapper mapper, SieveProcessor sieveProcessor)
            {
                _mapper = mapper;
                _db = db;
                _sieveProcessor = sieveProcessor;
            }

            public async Task<PagedList<ReportRequestDto>> Handle(ReportRequestListQuery request, CancellationToken cancellationToken)
            {
                if (request.QueryParameters == null)
                {
                    // log error
                    throw new ApiException("Invalid query parameters.");
                }

                var collection = _db.ReportRequests
                    as IQueryable<ReportRequest>;

                var sieveModel = new SieveModel
                {
                    Sorts = request.QueryParameters.SortOrder ?? "ReportId",
                    Filters = request.QueryParameters.Filters
                };

                var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
                var dtoCollection = appliedCollection
                    .ProjectTo<ReportRequestDto>(_mapper.ConfigurationProvider);

                return await PagedList<ReportRequestDto>.CreateAsync(dtoCollection,
                    request.QueryParameters.PageNumber,
                    request.QueryParameters.PageSize);
            }
        }
    }
}
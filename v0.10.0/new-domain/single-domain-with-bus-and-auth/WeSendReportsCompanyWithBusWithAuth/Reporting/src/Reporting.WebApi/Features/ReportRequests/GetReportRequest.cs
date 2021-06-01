namespace Reporting.WebApi.Features.ReportRequests
{
    using Reporting.Core.Dtos.ReportRequest;
    using Reporting.Core.Exceptions;
    using Reporting.Infrastructure.Contexts;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public static class GetReportRequest
    {
        public class ReportRequestQuery : IRequest<ReportRequestDto>
        {
            public Guid ReportId { get; set; }

            public ReportRequestQuery(Guid reportId)
            {
                ReportId = reportId;
            }
        }

        public class Handler : IRequestHandler<ReportRequestQuery, ReportRequestDto>
        {
            private readonly ReportingDbContext _db;
            private readonly IMapper _mapper;

            public Handler(ReportingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<ReportRequestDto> Handle(ReportRequestQuery request, CancellationToken cancellationToken)
            {
                // add logger (and a try catch with logger so i can cap the unexpected info)........ unless this happens in my logger decorator that i am going to add?

                var result = await _db.ReportRequests
                    .ProjectTo<ReportRequestDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(r => r.ReportId == request.ReportId);

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
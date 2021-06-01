namespace Reporting.WebApi.Features.ReportRequests
{
    using Reporting.Core.Entities;
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

    public static class DeleteReportRequest
    {
        public class DeleteReportRequestCommand : IRequest<bool>
        {
            public Guid ReportId { get; set; }

            public DeleteReportRequestCommand(Guid reportRequest)
            {
                ReportId = reportRequest;
            }
        }

        public class Handler : IRequestHandler<DeleteReportRequestCommand, bool>
        {
            private readonly ReportingDbContext _db;
            private readonly IMapper _mapper;

            public Handler(ReportingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<bool> Handle(DeleteReportRequestCommand request, CancellationToken cancellationToken)
            {
                // add logger (and a try catch with logger so i can cap the unexpected info)........ unless this happens in my logger decorator that i am going to add?

                var recordToDelete = await _db.ReportRequests
                    .FirstOrDefaultAsync(r => r.ReportId == request.ReportId);

                if (recordToDelete == null)
                {
                    // log error
                    throw new KeyNotFoundException();
                }

                _db.ReportRequests.Remove(recordToDelete);
                var saveSuccessful = await _db.SaveChangesAsync() > 0;

                if (!saveSuccessful)
                {
                    // add log
                    throw new Exception("Unable to save the new record. Please check the logs for more information.");
                }

                return true;
            }
        }
    }
}
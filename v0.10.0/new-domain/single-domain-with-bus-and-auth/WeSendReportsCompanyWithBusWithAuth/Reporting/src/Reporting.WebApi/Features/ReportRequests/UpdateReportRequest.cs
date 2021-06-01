namespace Reporting.WebApi.Features.ReportRequests
{
    using Reporting.Core.Entities;
    using Reporting.Core.Dtos.ReportRequest;
    using Reporting.Core.Exceptions;
    using Reporting.Infrastructure.Contexts;
    using Reporting.WebApi.Features.ReportRequests.Validators;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public static class UpdateReportRequest
    {
        public class UpdateReportRequestCommand : IRequest<bool>
        {
            public Guid ReportId { get; set; }
            public ReportRequestForUpdateDto ReportRequestToUpdate { get; set; }

            public UpdateReportRequestCommand(Guid reportRequest, ReportRequestForUpdateDto reportRequestToUpdate)
            {
                ReportId = reportRequest;
                ReportRequestToUpdate = reportRequestToUpdate;
            }
        }

        public class CustomUpdateReportRequestValidation : ReportRequestForManipulationDtoValidator<ReportRequestForUpdateDto>
        {
            public CustomUpdateReportRequestValidation()
            {
            }
        }

        public class Handler : IRequestHandler<UpdateReportRequestCommand, bool>
        {
            private readonly ReportingDbContext _db;
            private readonly IMapper _mapper;

            public Handler(ReportingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<bool> Handle(UpdateReportRequestCommand request, CancellationToken cancellationToken)
            {
                // add logger or use decorator

                var recordToUpdate = await _db.ReportRequests
                    .FirstOrDefaultAsync(r => r.ReportId == request.ReportId);

                if (recordToUpdate == null)
                {
                    // log error
                    throw new KeyNotFoundException();
                }

                _mapper.Map(request.ReportRequestToUpdate, recordToUpdate);
                var saveSuccessful = await _db.SaveChangesAsync() > 0;

                if (!saveSuccessful)
                {
                    // add log
                    throw new Exception("Unable to save the requested changes. Please check the logs for more information.");
                }

                return true;
            }
        }
    }
}
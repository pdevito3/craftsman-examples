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
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public static class PatchReportRequest
    {
        public class PatchReportRequestCommand : IRequest<bool>
        {
            public Guid ReportId { get; set; }
            public JsonPatchDocument<ReportRequestForUpdateDto> PatchDoc { get; set; }

            public PatchReportRequestCommand(Guid reportRequest, JsonPatchDocument<ReportRequestForUpdateDto> patchDoc)
            {
                ReportId = reportRequest;
                PatchDoc = patchDoc;
            }
        }

        public class CustomPatchReportRequestValidation : ReportRequestForManipulationDtoValidator<ReportRequestForUpdateDto>
        {
            public CustomPatchReportRequestValidation()
            {
            }
        }

        public class Handler : IRequestHandler<PatchReportRequestCommand, bool>
        {
            private readonly ReportingDbContext _db;
            private readonly IMapper _mapper;

            public Handler(ReportingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<bool> Handle(PatchReportRequestCommand request, CancellationToken cancellationToken)
            {
                // add logger or use decorator
                if (request.PatchDoc == null)
                {
                    // log error
                    throw new ApiException("Invalid patch document.");
                }

                var reportRequestToUpdate = await _db.ReportRequests
                    .FirstOrDefaultAsync(r => r.ReportId == request.ReportId);

                if (reportRequestToUpdate == null)
                {
                    // log error
                    throw new KeyNotFoundException();
                }

                var reportRequestToPatch = _mapper.Map<ReportRequestForUpdateDto>(reportRequestToUpdate); // map the reportRequest we got from the database to an updatable reportRequest model
                request.PatchDoc.ApplyTo(reportRequestToPatch); // apply patchdoc updates to the updatable reportRequest

                var validationResults = new CustomPatchReportRequestValidation().Validate(reportRequestToPatch);
                if (!validationResults.IsValid)
                {
                    throw new ValidationException(validationResults.Errors);
                }

                _mapper.Map(reportRequestToPatch, reportRequestToUpdate);
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
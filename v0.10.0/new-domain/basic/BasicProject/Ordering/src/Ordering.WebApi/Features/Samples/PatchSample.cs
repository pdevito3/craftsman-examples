namespace Ordering.WebApi.Features.Samples
{
    using Ordering.Core.Entities;
    using Ordering.Core.Dtos.Sample;
    using Ordering.Core.Exceptions;
    using Ordering.Infrastructure.Contexts;
    using Ordering.WebApi.Features.Samples.Validators;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public static class PatchSample
    {
        public class PatchSampleCommand : IRequest<bool>
        {
            public Guid SampleId { get; set; }
            public JsonPatchDocument<SampleForUpdateDto> PatchDoc { get; set; }

            public PatchSampleCommand(Guid sample, JsonPatchDocument<SampleForUpdateDto> patchDoc)
            {
                SampleId = sample;
                PatchDoc = patchDoc;
            }
        }

        public class CustomPatchSampleValidation : SampleForManipulationDtoValidator<SampleForUpdateDto>
        {
            public CustomPatchSampleValidation()
            {
            }
        }

        public class Handler : IRequestHandler<PatchSampleCommand, bool>
        {
            private readonly OrderingDbContext _db;
            private readonly IMapper _mapper;

            public Handler(OrderingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<bool> Handle(PatchSampleCommand request, CancellationToken cancellationToken)
            {
                // add logger or use decorator
                if (request.PatchDoc == null)
                {
                    // log error
                    throw new ApiException("Invalid patch document.");
                }

                var sampleToUpdate = await _db.Samples
                    .FirstOrDefaultAsync(s => s.SampleId == request.SampleId);

                if (sampleToUpdate == null)
                {
                    // log error
                    throw new KeyNotFoundException();
                }

                var sampleToPatch = _mapper.Map<SampleForUpdateDto>(sampleToUpdate); // map the sample we got from the database to an updatable sample model
                request.PatchDoc.ApplyTo(sampleToPatch); // apply patchdoc updates to the updatable sample

                var validationResults = new CustomPatchSampleValidation().Validate(sampleToPatch);
                if (!validationResults.IsValid)
                {
                    throw new ValidationException(validationResults.Errors);
                }

                _mapper.Map(sampleToPatch, sampleToUpdate);
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
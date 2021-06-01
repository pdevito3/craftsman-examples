namespace Ordering.WebApi.Features.Patients
{
    using Ordering.Core.Entities;
    using Ordering.Core.Dtos.Patient;
    using Ordering.Core.Exceptions;
    using Ordering.Infrastructure.Contexts;
    using Ordering.WebApi.Features.Patients.Validators;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public static class PatchPatient
    {
        public class PatchPatientCommand : IRequest<bool>
        {
            public Guid PatientId { get; set; }
            public JsonPatchDocument<PatientForUpdateDto> PatchDoc { get; set; }

            public PatchPatientCommand(Guid patient, JsonPatchDocument<PatientForUpdateDto> patchDoc)
            {
                PatientId = patient;
                PatchDoc = patchDoc;
            }
        }

        public class CustomPatchPatientValidation : PatientForManipulationDtoValidator<PatientForUpdateDto>
        {
            public CustomPatchPatientValidation()
            {
            }
        }

        public class Handler : IRequestHandler<PatchPatientCommand, bool>
        {
            private readonly OrderingDbContext _db;
            private readonly IMapper _mapper;

            public Handler(OrderingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<bool> Handle(PatchPatientCommand request, CancellationToken cancellationToken)
            {
                // add logger or use decorator
                if (request.PatchDoc == null)
                {
                    // log error
                    throw new ApiException("Invalid patch document.");
                }

                var patientToUpdate = await _db.Patients
                    .FirstOrDefaultAsync(p => p.PatientId == request.PatientId);

                if (patientToUpdate == null)
                {
                    // log error
                    throw new KeyNotFoundException();
                }

                var patientToPatch = _mapper.Map<PatientForUpdateDto>(patientToUpdate); // map the patient we got from the database to an updatable patient model
                request.PatchDoc.ApplyTo(patientToPatch); // apply patchdoc updates to the updatable patient

                var validationResults = new CustomPatchPatientValidation().Validate(patientToPatch);
                if (!validationResults.IsValid)
                {
                    throw new ValidationException(validationResults.Errors);
                }

                _mapper.Map(patientToPatch, patientToUpdate);
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
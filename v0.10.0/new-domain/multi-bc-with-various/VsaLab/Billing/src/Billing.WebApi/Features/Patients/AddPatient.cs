namespace Billing.WebApi.Features.Patients
{
    using Billing.Core.Entities;
    using Billing.Core.Dtos.Patient;
    using Billing.Core.Exceptions;
    using Billing.Infrastructure.Contexts;
    using Billing.WebApi.Features.Patients.Validators;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public static class AddPatient
    {
        public class AddPatientCommand : IRequest<PatientDto>
        {
            public PatientForCreationDto PatientToAdd { get; set; }

            public AddPatientCommand(PatientForCreationDto patientToAdd)
            {
                PatientToAdd = patientToAdd;
            }
        }

        public class CustomCreatePatientValidation : PatientForManipulationDtoValidator<PatientForCreationDto>
        {
            public CustomCreatePatientValidation()
            {
            }
        }

        public class Handler : IRequestHandler<AddPatientCommand, PatientDto>
        {
            private readonly BillingDbContext _db;
            private readonly IMapper _mapper;

            public Handler(BillingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<PatientDto> Handle(AddPatientCommand request, CancellationToken cancellationToken)
            {
                if (await _db.Patients.AnyAsync(p => p.PatientId == request.PatientToAdd.PatientId))
                {
                    throw new ConflictException("Patient already exists with this primary key.");
                }

                var patient = _mapper.Map<Patient> (request.PatientToAdd);
                _db.Patients.Add(patient);
                var saveSuccessful = await _db.SaveChangesAsync() > 0;

                if (saveSuccessful)
                {
                    return await _db.Patients
                        .ProjectTo<PatientDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(p => p.PatientId == patient.PatientId);
                }
                else
                {
                    // add log
                    throw new Exception("Unable to save the new record. Please check the logs for more information.");
                }
            }
        }
    }
}
namespace Ordering.WebApi.Features.Patients
{
    using Ordering.Core.Entities;
    using Ordering.Core.Dtos.Patient;
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

    public static class DeletePatient
    {
        public class DeletePatientCommand : IRequest<bool>
        {
            public Guid PatientId { get; set; }

            public DeletePatientCommand(Guid patient)
            {
                PatientId = patient;
            }
        }

        public class Handler : IRequestHandler<DeletePatientCommand, bool>
        {
            private readonly OrderingDbContext _db;
            private readonly IMapper _mapper;

            public Handler(OrderingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<bool> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
            {
                // add logger (and a try catch with logger so i can cap the unexpected info)........ unless this happens in my logger decorator that i am going to add?

                var recordToDelete = await _db.Patients
                    .FirstOrDefaultAsync(p => p.PatientId == request.PatientId);

                if (recordToDelete == null)
                {
                    // log error
                    throw new KeyNotFoundException();
                }

                _db.Patients.Remove(recordToDelete);
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
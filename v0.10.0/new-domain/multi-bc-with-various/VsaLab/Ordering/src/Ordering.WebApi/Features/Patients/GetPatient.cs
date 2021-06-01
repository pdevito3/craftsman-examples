namespace Ordering.WebApi.Features.Patients
{
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

    public static class GetPatient
    {
        public class PatientQuery : IRequest<PatientDto>
        {
            public Guid PatientId { get; set; }

            public PatientQuery(Guid patientId)
            {
                PatientId = patientId;
            }
        }

        public class Handler : IRequestHandler<PatientQuery, PatientDto>
        {
            private readonly OrderingDbContext _db;
            private readonly IMapper _mapper;

            public Handler(OrderingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<PatientDto> Handle(PatientQuery request, CancellationToken cancellationToken)
            {
                // add logger (and a try catch with logger so i can cap the unexpected info)........ unless this happens in my logger decorator that i am going to add?

                var result = await _db.Patients
                    .ProjectTo<PatientDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(p => p.PatientId == request.PatientId);

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
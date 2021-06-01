namespace Billing.WebApi.Features.Patients
{
    using Billing.Core.Entities;
    using Billing.Core.Dtos.Patient;
    using Billing.Core.Exceptions;
    using Billing.Infrastructure.Contexts;
    using Billing.Core.Wrappers;
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

    public static class GetPatientList
    {
        public class PatientListQuery : IRequest<PagedList<PatientDto>>
        {
            public PatientParametersDto QueryParameters { get; set; }

            public PatientListQuery(PatientParametersDto queryParameters)
            {
                QueryParameters = queryParameters;
            }
        }

        public class Handler : IRequestHandler<PatientListQuery, PagedList<PatientDto>>
        {
            private readonly BillingDbContext _db;
            private readonly SieveProcessor _sieveProcessor;
            private readonly IMapper _mapper;

            public Handler(BillingDbContext db, IMapper mapper, SieveProcessor sieveProcessor)
            {
                _mapper = mapper;
                _db = db;
                _sieveProcessor = sieveProcessor;
            }

            public async Task<PagedList<PatientDto>> Handle(PatientListQuery request, CancellationToken cancellationToken)
            {
                if (request.QueryParameters == null)
                {
                    // log error
                    throw new ApiException("Invalid query parameters.");
                }

                var collection = _db.Patients
                    as IQueryable<Patient>;

                var sieveModel = new SieveModel
                {
                    Sorts = request.QueryParameters.SortOrder ?? "PatientId",
                    Filters = request.QueryParameters.Filters
                };

                var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
                var dtoCollection = appliedCollection
                    .ProjectTo<PatientDto>(_mapper.ConfigurationProvider);

                return await PagedList<PatientDto>.CreateAsync(dtoCollection,
                    request.QueryParameters.PageNumber,
                    request.QueryParameters.PageSize);
            }
        }
    }
}
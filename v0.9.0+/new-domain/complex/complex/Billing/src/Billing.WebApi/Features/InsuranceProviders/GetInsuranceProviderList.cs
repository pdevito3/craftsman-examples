namespace Billing.WebApi.Features.InsuranceProviders
{
    using Billing.Core.Entities;
    using Billing.Core.Dtos.InsuranceProvider;
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

    public class GetInsuranceProviderList
    {
        public class InsuranceProviderListQuery : IRequest<PagedList<InsuranceProviderDto>>
        {
            public InsuranceProviderParametersDto QueryParameters { get; set; }

            public InsuranceProviderListQuery(InsuranceProviderParametersDto queryParameters)
            {
                QueryParameters = queryParameters;
            }
        }

        public class Handler : IRequestHandler<InsuranceProviderListQuery, PagedList<InsuranceProviderDto>>
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

            public async Task<PagedList<InsuranceProviderDto>> Handle(InsuranceProviderListQuery request, CancellationToken cancellationToken)
            {
                if (request.QueryParameters == null)
                {
                    // log error
                    throw new ApiException("Invalid query parameters.");
                }

                // include marker -- to accommodate adding includes with craftsman commands, the next line must stay as `var result = await _db.InsuranceProviders`. -- do not delete this comment
                var collection = _db.InsuranceProviders
                    as IQueryable<InsuranceProvider>;

                var sieveModel = new SieveModel
                {
                    Sorts = request.QueryParameters.SortOrder ?? "InsuranceProviderId",
                    Filters = request.QueryParameters.Filters
                };

                var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
                var dtoCollection = appliedCollection
                    .ProjectTo<InsuranceProviderDto>(_mapper.ConfigurationProvider);

                return await PagedList<InsuranceProviderDto>.CreateAsync(dtoCollection,
                    request.QueryParameters.PageNumber,
                    request.QueryParameters.PageSize);
            }
        }
    }
}
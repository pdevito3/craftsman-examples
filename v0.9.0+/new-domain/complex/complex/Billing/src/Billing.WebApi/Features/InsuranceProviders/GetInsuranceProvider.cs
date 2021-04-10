namespace Billing.WebApi.Features.InsuranceProviders
{
    using Billing.Core.Dtos.InsuranceProvider;
    using Billing.Core.Exceptions;
    using Billing.Infrastructure.Contexts;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class GetInsuranceProvider
    {
        public class InsuranceProviderQuery : IRequest<InsuranceProviderDto>
        {
            public Guid InsuranceProviderId { get; set; }

            public InsuranceProviderQuery(Guid insuranceProviderId)
            {
                InsuranceProviderId = insuranceProviderId;
            }
        }

        public class Handler : IRequestHandler<InsuranceProviderQuery, InsuranceProviderDto>
        {
            private readonly BillingDbContext _db;
            private readonly IMapper _mapper;

            public Handler(BillingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<InsuranceProviderDto> Handle(InsuranceProviderQuery request, CancellationToken cancellationToken)
            {
                // add logger (and a try catch with logger so i can cap the unexpected info)........ unless this happens in my logger decorator that i am going to add?

                // include marker -- to accommodate adding includes with craftsman commands, the next line must stay as `var result = await _db.InsuranceProviders`. -- do not delete this comment
                var result = await _db.InsuranceProviders
                    .ProjectTo<InsuranceProviderDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(i => i.InsuranceProviderId == request.InsuranceProviderId);

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
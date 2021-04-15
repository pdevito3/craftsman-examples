namespace Billing.WebApi.Features.InsuranceProviders
{
    using Billing.Core.Entities;
    using Billing.Core.Dtos.InsuranceProvider;
    using Billing.Core.Exceptions;
    using Billing.Infrastructure.Contexts;
    using Billing.WebApi.Features.InsuranceProviders.Validators;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class AddInsuranceProvider
    {
        public class AddInsuranceProviderCommand : IRequest<InsuranceProviderDto>
        {
            public InsuranceProviderForCreationDto InsuranceProviderToAdd { get; set; }

            public AddInsuranceProviderCommand(InsuranceProviderForCreationDto insuranceProviderToAdd)
            {
                InsuranceProviderToAdd = insuranceProviderToAdd;
            }
        }

        public class CustomCreateInsuranceProviderValidation : InsuranceProviderForManipulationDtoValidator<InsuranceProviderForCreationDto>
        {
            public CustomCreateInsuranceProviderValidation()
            {
            }
        }

        public class Handler : IRequestHandler<AddInsuranceProviderCommand, InsuranceProviderDto>
        {
            private readonly BillingDbContext _db;
            private readonly IMapper _mapper;

            public Handler(BillingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<InsuranceProviderDto> Handle(AddInsuranceProviderCommand request, CancellationToken cancellationToken)
            {
                var insuranceProvider = _mapper.Map<InsuranceProvider> (request.InsuranceProviderToAdd);
                _db.InsuranceProviders.Add(insuranceProvider);
                var saveSuccessful = await _db.SaveChangesAsync() > 0;

                if (saveSuccessful)
                {
                    // include marker -- to accommodate adding includes with craftsman commands, the next line must stay as `var result = await _db.InsuranceProviders`. -- do not delete this comment
                    return await _db.InsuranceProviders
                        .ProjectTo<InsuranceProviderDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(i => i.InsuranceProviderId == insuranceProvider.InsuranceProviderId);
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
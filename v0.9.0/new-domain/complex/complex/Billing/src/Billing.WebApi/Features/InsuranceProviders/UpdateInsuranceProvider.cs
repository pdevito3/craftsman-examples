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

    public class UpdateInsuranceProvider
    {
        public class UpdateInsuranceProviderCommand : IRequest<bool>
        {
            public Guid InsuranceProviderId { get; set; }
            public InsuranceProviderForUpdateDto InsuranceProviderToUpdate { get; set; }

            public UpdateInsuranceProviderCommand(Guid insuranceProvider, InsuranceProviderForUpdateDto insuranceProviderToUpdate)
            {
                InsuranceProviderId = insuranceProvider;
                InsuranceProviderToUpdate = insuranceProviderToUpdate;
            }
        }

        public class CustomUpdateInsuranceProviderValidation : InsuranceProviderForManipulationDtoValidator<InsuranceProviderForUpdateDto>
        {
            public CustomUpdateInsuranceProviderValidation()
            {
            }
        }

        public class Handler : IRequestHandler<UpdateInsuranceProviderCommand, bool>
        {
            private readonly BillingDbContext _db;
            private readonly IMapper _mapper;

            public Handler(BillingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<bool> Handle(UpdateInsuranceProviderCommand request, CancellationToken cancellationToken)
            {
                // add logger (and a try catch with logger so i can cap the unexpected info)........ unless this happens in my logger decorator that i am going to add?

                var recordToUpdate = await _db.InsuranceProviders
                    .FirstOrDefaultAsync(i => i.InsuranceProviderId == request.InsuranceProviderId);

                if (recordToUpdate == null)
                {
                    // log error
                    throw new KeyNotFoundException();
                }

                _mapper.Map(request.InsuranceProviderToUpdate, recordToUpdate);
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
namespace Billing.WebApi.Features.InsuranceProviders
{
    using Billing.Core.Entities;
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

    public class DeleteInsuranceProvider
    {
        public class DeleteInsuranceProviderCommand : IRequest<bool>
        {
            public Guid InsuranceProviderId { get; set; }

            public DeleteInsuranceProviderCommand(Guid insuranceProvider)
            {
                InsuranceProviderId = insuranceProvider;
            }
        }

        public class Handler : IRequestHandler<DeleteInsuranceProviderCommand, bool>
        {
            private readonly BillingDbContext _db;
            private readonly IMapper _mapper;

            public Handler(BillingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<bool> Handle(DeleteInsuranceProviderCommand request, CancellationToken cancellationToken)
            {
                // add logger (and a try catch with logger so i can cap the unexpected info)........ unless this happens in my logger decorator that i am going to add?

                var recordToDelete = await _db.InsuranceProviders
                    .FirstOrDefaultAsync(i => i.InsuranceProviderId == request.InsuranceProviderId);

                if (recordToDelete == null)
                {
                    // log error
                    throw new KeyNotFoundException();
                }

                _db.InsuranceProviders.Remove(recordToDelete);
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
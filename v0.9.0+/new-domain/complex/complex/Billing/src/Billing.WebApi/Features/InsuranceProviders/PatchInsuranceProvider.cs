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
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class PatchInsuranceProvider
    {
        public class PatchInsuranceProviderCommand : IRequest<bool>
        {
            public Guid InsuranceProviderId { get; set; }
            public JsonPatchDocument<InsuranceProviderForUpdateDto> PatchDoc { get; set; }

            public PatchInsuranceProviderCommand(Guid insuranceProvider, JsonPatchDocument<InsuranceProviderForUpdateDto> patchDoc)
            {
                InsuranceProviderId = insuranceProvider;
                PatchDoc = patchDoc;
            }
        }

        public class CustomPatchInsuranceProviderValidation : InsuranceProviderForManipulationDtoValidator<InsuranceProviderForUpdateDto>
        {
            public CustomPatchInsuranceProviderValidation()
            {
            }
        }

        public class Handler : IRequestHandler<PatchInsuranceProviderCommand, bool>
        {
            private readonly BillingDbContext _db;
            private readonly IMapper _mapper;

            public Handler(BillingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<bool> Handle(PatchInsuranceProviderCommand request, CancellationToken cancellationToken)
            {
                // add logger (and a try catch with logger so i can cap the unexpected info)........ unless this happens in my logger decorator that i am going to add?
                if (request.PatchDoc == null)
                {
                    // log error
                    throw new ApiException("Invalid patch document.");
                }

                var insuranceProviderToUpdate = await _db.InsuranceProviders
                    .FirstOrDefaultAsync(i => i.InsuranceProviderId == request.InsuranceProviderId);

                if (insuranceProviderToUpdate == null)
                {
                    // log error
                    throw new KeyNotFoundException();
                }
                
                var insuranceProviderToPatch = _mapper.Map<InsuranceProviderForUpdateDto>(insuranceProviderToUpdate); // map the insuranceProvider we got from the database to an updatable insuranceProvider model
                request.PatchDoc.ApplyTo(insuranceProviderToPatch); // apply patchdoc updates to the updatable insuranceProvider
                
                var validationResults = new CustomPatchInsuranceProviderValidation().Validate(insuranceProviderToPatch);
                if (!validationResults.IsValid)
                {
                    throw new ValidationException(validationResults.Errors);
                }

                _mapper.Map(insuranceProviderToPatch, insuranceProviderToUpdate);
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
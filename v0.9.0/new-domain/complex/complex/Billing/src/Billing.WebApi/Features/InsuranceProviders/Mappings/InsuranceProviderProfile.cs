namespace Billing.WebApi.Features.InsuranceProviders.Mappings
{
    using Billing.Core.Dtos.InsuranceProvider;
    using AutoMapper;
    using Billing.Core.Entities;

    public class InsuranceProviderProfile : Profile
    {
        public InsuranceProviderProfile()
        {
            //createmap<to this, from this>
            CreateMap<InsuranceProvider, InsuranceProviderDto>()
                .ReverseMap();
            CreateMap<InsuranceProviderForCreationDto, InsuranceProvider>();
            CreateMap<InsuranceProviderForUpdateDto, InsuranceProvider>()
                .ReverseMap();
        }
    }
}
namespace Ordering.WebApi.Features.Samples.Mappings
{
    using Ordering.Core.Dtos.Sample;
    using AutoMapper;
    using Ordering.Core.Entities;

    public class SampleProfile : Profile
    {
        public SampleProfile()
        {
            //createmap<to this, from this>
            CreateMap<Sample, SampleDto>()
                .ReverseMap();
            CreateMap<SampleForCreationDto, Sample>();
            CreateMap<SampleForUpdateDto, Sample>()
                .ReverseMap();
        }
    }
}
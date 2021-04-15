namespace Ordering.WebApi.Features.Patients.Mappings
{
    using Ordering.Core.Dtos.Patient;
    using AutoMapper;
    using Ordering.Core.Entities;

    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            //createmap<to this, from this>
            CreateMap<Patient, PatientDto>()
                .ReverseMap();
            CreateMap<PatientForCreationDto, Patient>();
            CreateMap<PatientForUpdateDto, Patient>()
                .ReverseMap();
        }
    }
}
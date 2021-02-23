namespace Application.Mappings
{
    using Application.Dtos.Patient;
    using AutoMapper;
    using Domain.Entities;

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
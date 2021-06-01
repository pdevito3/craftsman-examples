namespace Billing.WebApi.Features.Patients.Mappings
{
    using Billing.Core.Dtos.Patient;
    using AutoMapper;
    using Billing.Core.Entities;

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
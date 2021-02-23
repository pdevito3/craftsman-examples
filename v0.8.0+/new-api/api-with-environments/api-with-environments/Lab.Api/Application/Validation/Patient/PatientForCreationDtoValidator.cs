namespace Application.Validation.Patient
{
    using Application.Dtos.Patient;
    using FluentValidation;

    public class PatientForCreationDtoValidator: PatientForManipulationDtoValidator<PatientForCreationDto>
    {
        public PatientForCreationDtoValidator()
        {
            // add fluent validation rules that should only be run on creation operations here
            //https://fluentvalidation.net/
        }
    }
}
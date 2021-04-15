namespace Application.Validation.Patient
{
    using Application.Dtos.Patient;
    using FluentValidation;

    public class PatientForUpdateDtoValidator: PatientForManipulationDtoValidator<PatientForUpdateDto>
    {
        public PatientForUpdateDtoValidator()
        {
            // add fluent validation rules that should only be run on update operations here
            //https://fluentvalidation.net/
        }
    }
}
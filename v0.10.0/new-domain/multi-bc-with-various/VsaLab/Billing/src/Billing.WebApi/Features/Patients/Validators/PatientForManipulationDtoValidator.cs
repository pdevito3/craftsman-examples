namespace Billing.WebApi.Features.Patients.Validators
{
    using Billing.Core.Dtos.Patient;
    using FluentValidation;
    using System;

    public class PatientForManipulationDtoValidator<T> : AbstractValidator<T> where T : PatientForManipulationDto
    {
        public PatientForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
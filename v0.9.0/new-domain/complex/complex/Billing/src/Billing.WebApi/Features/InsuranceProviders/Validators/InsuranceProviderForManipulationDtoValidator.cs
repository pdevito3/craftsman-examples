namespace Billing.WebApi.Features.InsuranceProviders.Validators
{
    using Billing.Core.Dtos.InsuranceProvider;
    using FluentValidation;
    using System;

    public class InsuranceProviderForManipulationDtoValidator<T> : AbstractValidator<T> where T : InsuranceProviderForManipulationDto
    {
        public InsuranceProviderForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
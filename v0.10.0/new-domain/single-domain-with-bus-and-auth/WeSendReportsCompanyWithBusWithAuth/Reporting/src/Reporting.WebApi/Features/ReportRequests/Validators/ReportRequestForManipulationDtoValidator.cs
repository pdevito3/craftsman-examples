namespace Reporting.WebApi.Features.ReportRequests.Validators
{
    using Reporting.Core.Dtos.ReportRequest;
    using FluentValidation;
    using System;

    public class ReportRequestForManipulationDtoValidator<T> : AbstractValidator<T> where T : ReportRequestForManipulationDto
    {
        public ReportRequestForManipulationDtoValidator()
        {
            // add fluent validation rules that should be shared between creation and update operations here
            //https://fluentvalidation.net/
        }
    }
}
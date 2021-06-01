namespace Billing.Core.Dtos.Patient
{
    using Billing.Core.Dtos.Shared;

    public class PatientParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
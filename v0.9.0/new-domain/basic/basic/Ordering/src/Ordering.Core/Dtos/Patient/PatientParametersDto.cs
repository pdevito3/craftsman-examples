namespace Ordering.Core.Dtos.Patient
{
    using Ordering.Core.Dtos.Shared;

    public class PatientParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
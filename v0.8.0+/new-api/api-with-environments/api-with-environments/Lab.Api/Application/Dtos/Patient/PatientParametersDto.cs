namespace Application.Dtos.Patient
{
    using Application.Dtos.Shared;

    public class PatientParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
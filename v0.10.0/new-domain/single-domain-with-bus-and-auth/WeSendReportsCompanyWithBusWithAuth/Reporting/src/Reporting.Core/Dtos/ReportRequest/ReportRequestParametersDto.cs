namespace Reporting.Core.Dtos.ReportRequest
{
    using Reporting.Core.Dtos.Shared;

    public class ReportRequestParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
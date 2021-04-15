namespace Ordering.Core.Dtos.Sample
{
    using Ordering.Core.Dtos.Shared;

    public class SampleParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
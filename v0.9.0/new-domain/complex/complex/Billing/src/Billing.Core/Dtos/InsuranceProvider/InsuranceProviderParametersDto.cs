namespace Billing.Core.Dtos.InsuranceProvider
{
    using Billing.Core.Dtos.Shared;

    public class InsuranceProviderParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
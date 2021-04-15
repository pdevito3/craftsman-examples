namespace Billing.Core.Dtos.InsuranceProvider
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class InsuranceProviderDto 
    {
        public Guid InsuranceProviderId { get; set; }
        public string ProviderName { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
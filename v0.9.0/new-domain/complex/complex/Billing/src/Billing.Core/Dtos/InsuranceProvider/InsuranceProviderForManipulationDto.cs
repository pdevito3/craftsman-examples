namespace Billing.Core.Dtos.InsuranceProvider
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class InsuranceProviderForManipulationDto 
    {
        public string ProviderName { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
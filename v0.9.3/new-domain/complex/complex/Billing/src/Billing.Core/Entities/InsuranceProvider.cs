namespace Billing.Core.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Sieve.Attributes;

    [Table("InsuranceProvider")]
    public class InsuranceProvider
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public Guid InsuranceProviderId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string ProviderName { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
namespace Ordering.Core.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Sieve.Attributes;

    [Table("Sample")]
    public class Sample
    {
        [Key]
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public Guid SampleId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string ExternalId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string InternalId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string SampleType { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string ContainerType { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTimeOffset? CollectionDate { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTimeOffset? ArrivalDate { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int? Amount { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string AmountUnits { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
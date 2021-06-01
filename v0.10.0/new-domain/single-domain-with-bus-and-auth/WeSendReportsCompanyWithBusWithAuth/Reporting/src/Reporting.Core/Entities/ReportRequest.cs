namespace Reporting.Core.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Sieve.Attributes;

    [Table("ReportRequest")]
    public class ReportRequest
    {
        [Key]
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public Guid ReportId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Provider { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Target { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
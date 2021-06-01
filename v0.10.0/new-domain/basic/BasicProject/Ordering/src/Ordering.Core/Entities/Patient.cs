namespace Ordering.Core.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Sieve.Attributes;

    [Table("Patient")]
    public class Patient
    {
        [Key]
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public Guid PatientId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string ExternalId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string InternalId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string FirstName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string LastName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTimeOffset? Dob { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Sex { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Gender { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Race { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Ethnicity { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
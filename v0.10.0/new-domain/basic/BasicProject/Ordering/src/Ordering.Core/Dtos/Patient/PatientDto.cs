namespace Ordering.Core.Dtos.Patient
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PatientDto 
    {
        public Guid PatientId { get; set; }
        public string ExternalId { get; set; }
        public string InternalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset? Dob { get; set; }
        public string Sex { get; set; }
        public string Gender { get; set; }
        public string Race { get; set; }
        public string Ethnicity { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
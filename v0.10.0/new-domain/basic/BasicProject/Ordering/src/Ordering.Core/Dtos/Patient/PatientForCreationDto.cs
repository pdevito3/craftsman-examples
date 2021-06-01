namespace Ordering.Core.Dtos.Patient
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PatientForCreationDto : PatientForManipulationDto
    {
        public Guid PatientId { get; set; } = Guid.NewGuid();

        // add-on property marker - Do Not Delete This Comment
    }
}
namespace Ordering.Core.Dtos.Sample
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SampleForCreationDto : SampleForManipulationDto
    {
        public Guid SampleId { get; set; } = Guid.NewGuid();

        // add-on property marker - Do Not Delete This Comment
    }
}
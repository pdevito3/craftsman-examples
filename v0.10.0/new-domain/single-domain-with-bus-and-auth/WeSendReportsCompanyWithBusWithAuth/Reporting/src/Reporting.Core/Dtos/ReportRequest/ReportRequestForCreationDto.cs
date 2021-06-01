namespace Reporting.Core.Dtos.ReportRequest
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ReportRequestForCreationDto : ReportRequestForManipulationDto
    {
        public Guid ReportId { get; set; } = Guid.NewGuid();

        // add-on property marker - Do Not Delete This Comment
    }
}
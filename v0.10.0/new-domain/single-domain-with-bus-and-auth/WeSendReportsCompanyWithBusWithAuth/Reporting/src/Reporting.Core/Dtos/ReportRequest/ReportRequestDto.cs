namespace Reporting.Core.Dtos.ReportRequest
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ReportRequestDto 
    {
        public Guid ReportId { get; set; }
        public string Provider { get; set; }
        public string Target { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
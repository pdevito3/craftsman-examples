namespace Reporting.Core.Dtos.ReportRequest
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class ReportRequestForManipulationDto 
    {
        public string Provider { get; set; }
        public string Target { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
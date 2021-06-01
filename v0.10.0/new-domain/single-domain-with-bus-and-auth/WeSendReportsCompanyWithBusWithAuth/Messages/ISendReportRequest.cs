namespace Messages
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ISendReportRequest
    {
        Guid ReportId { get; set; }

        string Provider { get; set; }

        string Target { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
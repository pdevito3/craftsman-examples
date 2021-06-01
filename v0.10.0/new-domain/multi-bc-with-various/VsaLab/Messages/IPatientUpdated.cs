namespace Messages
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IPatientUpdated
    {
        Guid PatientId { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
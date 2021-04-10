namespace Ordering.FunctionalTests.TestUtilities
{
    public class ApiRoutes
    {
        public const string Base = "api";
        public const string Health = Base + "/health";        

public static class Patients
        {
            public const string PatientId = "{patientId}";
            public const string GetList = Base + "/patients";
            public const string GetRecord = Base + "/patients/" + PatientId;
            public const string Create = Base + "/patients";
            public const string Delete = Base + "/patients/" + PatientId;
            public const string Put = Base + "/patients/" + PatientId;
            public const string Patch = Base + "/patients/" + PatientId;
        }

public static class Samples
        {
            public const string SampleId = "{sampleId}";
            public const string GetList = Base + "/samples";
            public const string GetRecord = Base + "/samples/" + SampleId;
            public const string Create = Base + "/samples";
            public const string Delete = Base + "/samples/" + SampleId;
            public const string Put = Base + "/samples/" + SampleId;
            public const string Patch = Base + "/samples/" + SampleId;
        }

        // new api route marker - do not delete
    }
}
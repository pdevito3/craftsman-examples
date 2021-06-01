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

        // new api route marker - do not delete
    }
}
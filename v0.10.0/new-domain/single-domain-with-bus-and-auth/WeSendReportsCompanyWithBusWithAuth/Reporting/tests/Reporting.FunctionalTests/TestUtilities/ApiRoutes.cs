namespace Reporting.FunctionalTests.TestUtilities
{
    public class ApiRoutes
    {
        public const string Base = "api";
        public const string Health = Base + "/health";        

public static class ReportRequests
        {
            public const string ReportId = "{reportId}";
            public const string GetList = Base + "/reportRequests";
            public const string GetRecord = Base + "/reportRequests/" + ReportId;
            public const string Create = Base + "/reportRequests";
            public const string Delete = Base + "/reportRequests/" + ReportId;
            public const string Put = Base + "/reportRequests/" + ReportId;
            public const string Patch = Base + "/reportRequests/" + ReportId;
        }

        // new api route marker - do not delete
    }
}
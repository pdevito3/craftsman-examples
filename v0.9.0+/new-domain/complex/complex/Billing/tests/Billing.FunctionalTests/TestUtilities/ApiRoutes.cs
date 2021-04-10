namespace Billing.FunctionalTests.TestUtilities
{
    public class ApiRoutes
    {
        public const string Base = "api";
        public const string Health = Base + "/health";        

public static class InsuranceProviders
        {
            public const string InsuranceProviderId = "{insuranceProviderId}";
            public const string GetList = Base + "/insuranceProviders";
            public const string GetRecord = Base + "/insuranceProviders/" + InsuranceProviderId;
            public const string Create = Base + "/insuranceProviders";
            public const string Delete = Base + "/insuranceProviders/" + InsuranceProviderId;
            public const string Put = Base + "/insuranceProviders/" + InsuranceProviderId;
            public const string Patch = Base + "/insuranceProviders/" + InsuranceProviderId;
        }

        // new api route marker - do not delete
    }
}
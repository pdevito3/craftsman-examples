
namespace CarbonKitchen.Api.Tests.Helpers
{
    using System;
    using System.Dynamic;
    using System.Net;
    using System.Net.Http;

    public static class HttpClientExtensions
    {
        public static HttpClient AddAuth(this HttpClient client, string[] scopes)
        {
            dynamic data = new ExpandoObject();
            data.sub = Guid.NewGuid();
            data.scope = scopes;
            client.SetFakeBearerToken((object)data);

            return client;
        }
    }
}
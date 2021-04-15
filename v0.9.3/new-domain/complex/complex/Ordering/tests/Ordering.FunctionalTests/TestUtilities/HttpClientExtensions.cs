
namespace Ordering.FunctionalTests.TestUtilities
{
    using Microsoft.AspNetCore.JsonPatch;
    using Newtonsoft.Json;
    using System;
    using System.Dynamic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text;
    using System.Threading.Tasks;

    public static class HttpClientExtensions
    {
        public static HttpClient AddAuth(this HttpClient client, params string[] scopes)
        {
            dynamic data = new ExpandoObject();
            data.sub = Guid.NewGuid();
            data.scope = scopes;
            client.SetFakeBearerToken((object)data);

            return client;
        }

        public async static Task<HttpResponseMessage> GetRequestAsync(this HttpClient client, string url)
        {
            return await client.GetAsync(url).ConfigureAwait(false);
        }

        public async static Task<HttpResponseMessage> DeleteRequestAsync(this HttpClient client, string url)
        {
            return await client.DeleteAsync(url).ConfigureAwait(false);
        }

        public async static Task<HttpResponseMessage> PostJsonRequestAsync(this HttpClient client, string url, object value)
        {
            return await client.PostAsJsonAsync(url, value).ConfigureAwait(false);
        }

        public async static Task<HttpResponseMessage> PutJsonRequestAsync(this HttpClient client, string url, object value)
        {
            return await client.PutAsJsonAsync(url, value).ConfigureAwait(false);
        }

        public async static Task<HttpResponseMessage> PatchJsonRequestAsync<TModel>(this HttpClient client, string url, JsonPatchDocument<TModel> patchDoc)
            where TModel : class
        {
            var serializedRecipeToUpdate = JsonConvert.SerializeObject(patchDoc);

            var patchRequest = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = new StringContent(serializedRecipeToUpdate, Encoding.Unicode, "application/json")
            };

            return await client.SendAsync(patchRequest).ConfigureAwait(false);
        }
    }
}
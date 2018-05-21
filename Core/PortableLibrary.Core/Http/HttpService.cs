using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PortableLibrary.Core.Http
{
    public class HttpService : IHttpService
    {
        /// <inheritdoc />
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest requestObject,
            string language = null)
            where TRequest : class, new()
            where TResponse : class, new()
        {
            var serializerSettings =
                new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};

            var content = JsonConvert.SerializeObject(requestObject, serializerSettings);

            return await PostStringAsync<TResponse>(url, content, language).ConfigureAwait(false);
        }
        
        /// <inheritdoc />
        public async Task<TResponse> PostStringAsync<TResponse>(string url, string content,
            string language = null)
            where TResponse : class, new()
        {
            const string mediaType = "application/json";

            var serializerSettings =
                new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
                if (!string.IsNullOrWhiteSpace(language))
                    client.DefaultRequestHeaders.Add("Accept-Language", language);

                var stringContent = new StringContent(content, Encoding.UTF8, mediaType);
                var response = await client.PostAsync(url, stringContent, CancellationToken.None).ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new HttpPostException("Request has failed.");
                var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var responseObject = JsonConvert.DeserializeObject<TResponse>(data, serializerSettings);
                return responseObject;
            }
        }
    }
}
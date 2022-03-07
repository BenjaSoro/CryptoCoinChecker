namespace CryptoCheckerApp.Backend.Clients
{
    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class BaseApiClient : IBaseApiClient
    {
        public HttpClient HttpClient { get; }

        private readonly JsonSerializerSettings serializerSettings = new ()
        {
            Culture = CultureInfo.InvariantCulture,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public BaseApiClient(HttpClient httpClient)
        {
            this.HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<T> GetAsync<T>(Uri resourceUri)
        {
            var response = await this.HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, resourceUri))
                               .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            try
            {
                return JsonConvert.DeserializeObject<T>(responseContent, this.serializerSettings);
            }
            catch (Exception e)
            {
                throw new HttpRequestException(e.Message);
            }
        }
    }
}

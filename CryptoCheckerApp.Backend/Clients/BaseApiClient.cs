namespace CryptoCheckerApp.Backend.Clients
{
    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <inheritdoc />
    public class BaseApiClient : IBaseApiClient
    {
        /// <summary>
        /// Gets an instance of HttpClient.
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// Serializer settings to be used during deserialization.
        /// </summary>
        private readonly JsonSerializerSettings serializerSettings = new()
        {
            Culture = CultureInfo.InvariantCulture,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiClient"/> class.
        /// </summary>
        /// <param name="httpClient">
        /// HttpClient injected instance.
        /// </param>
        public BaseApiClient(HttpClient httpClient)
        {
            this.HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <inheritdoc />
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

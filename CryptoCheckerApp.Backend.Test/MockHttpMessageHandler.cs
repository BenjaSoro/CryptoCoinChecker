namespace CryptoCheckerApp.Backend.Test
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Mock class for HttpMessageHandler in order to be injected on HttpClient to make NSubstitute test the internal protected SendAsync method.
    /// Refer to https://dev.to/n_develop/mocking-the-httpclient-in-net-core-with-nsubstitute-k4j for more details.
    /// </summary>
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly string response;
        private readonly HttpStatusCode statusCode;

        public int NumberOfCalls { get; private set; }

        public string UriInput { get; private set; }

        public MockHttpMessageHandler(string response, HttpStatusCode statusCode)
        {
            this.response = response;
            this.statusCode = statusCode;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            NumberOfCalls++;
            UriInput = request.RequestUri.ToString();

            return new HttpResponseMessage
                       {
                           StatusCode = this.statusCode,
                           Content = new StringContent(this.response)
                       };
        }
    }
}
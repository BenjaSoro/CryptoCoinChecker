namespace CryptoCheckerApp.Backend.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using CryptoCheckerApp.Backend.Clients;
    using CryptoCheckerApp.Backend.GeckoApiDefinition.Entities;
    using CryptoCheckerApp.Backend.Services;

    using NUnit.Framework;

    public class GeckoServiceTests
    {
        private GeckoService geckoService;

        public IList<string> KnownCoinList { get; set; }

        [SetUp]
        public void Setup()
        {
            KnownCoinList = new List<string>()
                                {
                                    "Bitcoin",
                                    "Ethereum",
                                    "Aave",
                                    "Algorand",
                                    "Cardano",
                                    "Chainlink",
                                    "Dogecoin",
                                    "Ethereum Classic",
                                    "Litecoin",
                                    "Shiba Inu",
                                    "Solana",
                                    "Tezos",
                                    "Uniswap",
                                };
            KnownCoinList = KnownCoinList.Select(name => name.Replace(" ", "-").ToLowerInvariant()).ToList();
        }

        [Test]
        public void ConnectionIsRefused()
        {
            var httpClient = new HttpClient();
            var apiClient = new BaseApiClient(httpClient);

            this.geckoService = new GeckoService(apiClient);
            Assert.ThrowsAsync<HttpRequestException>(
                () => this.geckoService.GetMarketPricesFor("http://localhost", this.KnownCoinList));
        }
        
        [Test]
        public void UriNotFormatted()
        {
            var httpClient = new HttpClient();
            var apiClient = new BaseApiClient(httpClient);

            this.geckoService = new GeckoService(apiClient);
            Assert.ThrowsAsync<UriFormatException>(
                () => this.geckoService.GetMarketPricesFor("test", this.KnownCoinList));
        }

        [Test]
        public void ResponseNotFormatted()
        {
            var mockHttpHandler = new MockHttpMessageHandler("btc", HttpStatusCode.OK);

            var httpClient = new HttpClient(mockHttpHandler);
            var baseClient = new BaseApiClient(httpClient);
            this.geckoService = new GeckoService(baseClient);

            Assert.ThrowsAsync<HttpRequestException>(
                () => this.geckoService.GetMarketPricesFor("http://geckoendpoint.com", KnownCoinList));
        }
        
        [Test]
        public async Task VerifyMarketDefinitions()
        {
            const string CoinName = "Bitcoin";
            var responseMsg = new List<CoinMarketsDefinition> { new () { Id = "btc", Name = CoinName } };
            var responseSerialized = System.Text.Json.JsonSerializer.Serialize(responseMsg);

            var mockHttpHandler = new MockHttpMessageHandler(responseSerialized, HttpStatusCode.OK);

            var httpClient = new HttpClient(mockHttpHandler);
            var baseClient = new BaseApiClient(httpClient);
            this.geckoService = new GeckoService(baseClient);

            var coinMarketsDefinitions = await this.geckoService.GetMarketPricesFor("http://geckoendpoint.com", KnownCoinList);

            Assert.True(mockHttpHandler.UriInput.Contains("http://geckoendpoint.com"));
            foreach (var coin in this.KnownCoinList)
            {
                Assert.True(mockHttpHandler.UriInput.Contains(coin));
            }

            Assert.AreEqual(mockHttpHandler.NumberOfCalls, 1);
            Assert.True(coinMarketsDefinitions.Any(coinDef => string.Equals(coinDef.Name, CoinName, StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}
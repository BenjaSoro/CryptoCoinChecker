namespace CryptoCheckerApp.Backend.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CryptoCheckerApp.Backend.Clients;
    using CryptoCheckerApp.Backend.GeckoApiDefinition.Entities;

    /// <inheritdoc />
    public class GeckoService : IGeckoService
    {
        /// <summary>
        /// Base API Client injected instance.
        /// </summary>
        private readonly IBaseApiClient apiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeckoService"/> class.
        /// </summary>
        /// <param name="apiClient">
        /// Base API Client injected instance.
        /// </param>
        public GeckoService(IBaseApiClient apiClient)
        {
            this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

        /// <inheritdoc />
        public Task<IList<CoinMarketsDefinition>> GetMarketPricesFor(string marketPricesEndpoint, IEnumerable<string> coinIdList)
        {
            return this.apiClient.GetAsync<IList<CoinMarketsDefinition>>(
                QueryStringService.AppendQueryString(
                    marketPricesEndpoint,
                    new Dictionary<string, object>
                        {
                            { "vs_currency", "eur" },
                            { "ids", string.Join(",", coinIdList) },
                            { "order", "market_cap_desc" },
                            { "per_page", "100" },
                            { "page", "1" },
                            { "sparkline", false }
                        }));
        }
    }
}

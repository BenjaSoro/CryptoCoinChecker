namespace CryptoCheckerApp.Backend.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CryptoCheckerApp.Backend.Clients;
    using CryptoCheckerApp.Backend.GeckoApiDefinition.Entities;

    public class GeckoService : IGeckoService
    {
        private readonly IBaseApiClient apiClient;

        public GeckoService(IBaseApiClient apiClient)
        {
            this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

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

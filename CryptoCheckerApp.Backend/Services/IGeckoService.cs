namespace CryptoCheckerApp.Backend.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CryptoCheckerApp.Backend.GeckoApiDefinition.Entities;

    public interface IGeckoService
    {
        Task<IList<CoinMarketsDefinition>> GetMarketPricesFor(string marketPricesEndpoint, IEnumerable<string> coinIdList);
    }
}
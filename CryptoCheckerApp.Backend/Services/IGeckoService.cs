namespace CryptoCheckerApp.Backend.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CryptoCheckerApp.Backend.GeckoApiDefinition.Entities;

    /// <summary>
    /// Interface which defines the methods to interact with CoinGecko API.
    /// </summary>
    public interface IGeckoService
    {
        /// <summary>
        /// Method to create a request to Endpoint provided with a list of CoinIds.
        /// </summary>
        /// <param name="marketPricesEndpoint">
        /// The market prices endpoint.
        /// </param>
        /// <param name="coinIdList">
        /// The list of CoinIds.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<IList<CoinMarketsDefinition>> GetMarketPricesFor(string marketPricesEndpoint, IEnumerable<string> coinIdList);
    }
}
namespace CryptoCheckerApp.Backend.GeckoApiDefinition.Settings
{
    /// <summary>
    /// Class which represents the CoinGeckoAPI Settings from appsettings.json
    /// </summary>
    public class GeckoApiDefinitionSettings
    {
        /// <summary>
        /// Gets or sets the API base endpoint.
        /// </summary>
        public string ApiBaseEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the Coin Markets uri path.
        /// </summary>
        public string CoinMarketsPath { get; set; }
    }
}

namespace CryptoChecker.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using CryptoChecker.Models;
    using CryptoChecker.Utils;

    using CryptoCheckerApp.Domain.Entities;
    using Xamarin.Forms;

    /// <summary>
    /// Service which represents the methods which interact with Coin items.
    /// </summary>
    public class CoinService : ICoinService
    {
        /// <summary>
        /// The backend api end point.
        /// </summary>
        private const string ApiEndPoint = "api";

        /// <summary>
        /// The http client.
        /// </summary>
        private readonly HttpClient httpClient;

        public CoinService()
        {
            httpClient = DependencyService.Get<HttpClient>();
            httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        /// <summary>
        /// Gets the private list of available coins.
        /// </summary>
        public IList<CoinItem> CoinItems { get; private set; }

        /// <inheritdoc />
        public async Task<IList<CoinItem>> GetCoins()
        {
            if (CoinItems == null)
            {
                await GetAvailableCoinsFromBackEnd();
            }

            return CoinItems;
        }

        /// <inheritdoc />
        public IList<CoinItem> FilterByName(string name)
        {
            return CoinItems.Where(c => c.Coin.Name.ToLowerInvariant().Contains(name.ToLowerInvariant())).ToList();
        }

        /// <summary>
        /// The get available coins from back end.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task GetAvailableCoinsFromBackEnd()
        {
            var path = Path.Combine(BackendConstants.RootBackendUrl, ApiEndPoint, "GetAvailableCoins");

            using (var response = await httpClient.GetAsync(path).ConfigureAwait(false))
            {
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var coinList = JsonSerializer.Deserialize<IList<Coin>>(
                    json,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })?.Select(
                    c =>
                        {
                            var coinVm = new CoinItem
                                             {
                                                 Coin = c,
                                                 LogoPath = c.Name.ToLowerInvariant().Replace(" ", "-")
                                                     .Insert(c.Name.Length, ".png"),
                                                 ChangedIn24Hours = 0.00,
                                                 CurrentPrice = 0,
                                                 PercentBgColor = Color.Gray
                                             };
                            return coinVm;
                        }).ToList();
                CoinItems = coinList;
            }
        }
    }
}

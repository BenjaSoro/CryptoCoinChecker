namespace CryptoCheckerApp.Backend.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using CryptoCheckerApp.Backend.Clients;
    using CryptoCheckerApp.Backend.GeckoApiDefinition.EndPoints;
    using CryptoCheckerApp.Backend.GeckoApiDefinition.Entities.CoinGecko.Entities.Response.Coins;
    using CryptoCheckerApp.Backend.Hubs;
    using CryptoCheckerApp.Domain.Models;

    using Microsoft.Extensions.Options;

    using Serilog;

    public class GeckoBackgroundService : IBackgroundService
    {
        private readonly GeckoApiDefinitionSettings geckoApiDefinitionSettings;

        private readonly IBaseApiClient apiClient;

        private readonly ICoinService coinService;

        private readonly ISignalr signalr;

        private readonly BlockingCollection<UpdateCoinSignalMsg> newMarketPrices = new();
        public GeckoBackgroundService(IBaseApiClient apiClient, IOptions<GeckoApiDefinitionSettings> geckoApiDefinitionSettings, ICoinService coinService, ISignalr signalr)
        {
            if (geckoApiDefinitionSettings == null)
            {
                throw new ArgumentNullException(nameof(geckoApiDefinitionSettings));
            }

            this.geckoApiDefinitionSettings = geckoApiDefinitionSettings.Value;
            this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            this.coinService = coinService ?? throw new ArgumentNullException(nameof(coinService));
            this.signalr = signalr ?? throw new ArgumentNullException(nameof(signalr));
        }

        public async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            _ = Task.Run(() => this.ProcessSignalsQueue(cancellationToken), cancellationToken);

            var coinIdList = this.coinService.GetNameListAvailableCoins().Select(name => name.Replace(" ", "-")).ToList();

            var coinMarketsUri = new UriBuilder(this.geckoApiDefinitionSettings.ApiBaseEndpoint);
            coinMarketsUri.Path += this.geckoApiDefinitionSettings.CoinMarketsPath;
            while (!cancellationToken.IsCancellationRequested)
            {
                await this.GetMarketPricesFor(coinMarketsUri.Uri.ToString(), coinIdList).ConfigureAwait(false);
                await Task.Delay(TimeSpan.FromSeconds(20), cancellationToken).ConfigureAwait(false);
            }
        }

        private async Task ProcessSignalsQueue(CancellationToken cancellationToken)
        {
            foreach (var newPriceSignalMsg in this.newMarketPrices.GetConsumingEnumerable(cancellationToken))
            {
                await this.signalr.SendMessage(newPriceSignalMsg).ConfigureAwait(false);
            }
        }

        private async Task GetMarketPricesFor(string marketPricesEndpoint, IEnumerable<string> coinIdList)
        {
            try
            {
                var marketDataResult = await this.apiClient.GetAsync<IList<CoinMarketsDefinition>>(
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
                                                   })).ConfigureAwait(false);

                foreach (var coinMarket in marketDataResult)
                {
                    var coinSignalMsg = new UpdateCoinSignalMsg
                                {
                                    Symbol = coinMarket.Symbol,
                                    CurrentPrice = coinMarket.CurrentPrice,
                                    PriceChangePercentage24 = coinMarket.PriceChangePercentage24H
                                };
                    this.newMarketPrices.Add(coinSignalMsg);

                    Log.Information("The following result will be processed: "
                                    + $"{nameof(coinMarket.Name)}: {coinMarket.Name}, "
                                    + $"{nameof(coinMarket.Symbol)}: {coinMarket.Symbol}, "
                                    + $"{nameof(coinMarket.CurrentPrice)}: {coinMarket.CurrentPrice}, "
                                    + $"{nameof(coinMarket.PriceChangePercentage24H)}: {coinMarket.PriceChangePercentage24H}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

namespace CryptoCheckerApp.Backend.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using AutoMapper;

    using CryptoCheckerApp.Backend.GeckoApiDefinition.Settings;
    using CryptoCheckerApp.Backend.Hubs;
    using CryptoCheckerApp.Domain.Models;

    using Microsoft.Extensions.Options;

    using Serilog;

    /// <inheritdoc />
    public class GeckoBackgroundService : IBackgroundService
    {
        /// <summary>
        /// CoinGecko API settings.
        /// </summary>
        private readonly GeckoApiDefinitionSettings geckoApiDefinitionSettings;

        /// <summary>
        /// The injected coin service.
        /// </summary>
        private readonly ICoinService coinService;

        /// <summary>
        /// The injected gecko service.
        /// </summary>
        private readonly IGeckoService geckoService;

        /// <summary>
        /// The AutoMapper injected instance.
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// The SignalR injected instance.
        /// </summary>
        private readonly ISignalr signalr;

        /// <summary>
        /// Collection to be updated with new prices.
        /// </summary>
        private readonly BlockingCollection<UpdatedCoinSignalMsg> newMarketPrices = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="GeckoBackgroundService"/> class.
        /// </summary>
        /// <param name="geckoApiDefinitionSettings">
        /// CoinGecko API settings.
        /// </param>
        /// <param name="coinService">
        /// The injected coin service.
        /// </param>
        /// <param name="geckoService">
        /// The injected gecko service.
        /// </param>
        /// <param name="mapper">
        /// The AutoMapper injected instance.
        /// </param>
        /// <param name="signalr">
        /// The SignalR injected instance.
        /// </param>
        public GeckoBackgroundService(
            IOptions<GeckoApiDefinitionSettings> geckoApiDefinitionSettings,
            ICoinService coinService,
            IGeckoService geckoService,
            IMapper mapper,
            ISignalr signalr)
        {
            if (geckoApiDefinitionSettings == null)
            {
                throw new ArgumentNullException(nameof(geckoApiDefinitionSettings));
            }

            this.geckoApiDefinitionSettings = geckoApiDefinitionSettings.Value;
            this.coinService = coinService ?? throw new ArgumentNullException(nameof(coinService));
            this.geckoService = geckoService ?? throw new ArgumentNullException(nameof(geckoService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.signalr = signalr ?? throw new ArgumentNullException(nameof(signalr));
        }

        /// <inheritdoc />
        public async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            _ = Task.Run(() => this.ProcessSignalsQueue(cancellationToken), cancellationToken);

            var coinIdList = this.coinService.GetNameListAvailableCoins().Select(name => name.Replace(" ", "-")).ToList();

            var coinMarketsUri = new UriBuilder(this.geckoApiDefinitionSettings.ApiBaseEndpoint);
            coinMarketsUri.Path += this.geckoApiDefinitionSettings.CoinMarketsPath;

            while (!cancellationToken.IsCancellationRequested)
            {
                await this.UpdateMarketPrices(coinMarketsUri.Uri.ToString(), coinIdList, cancellationToken);

                await Task.Delay(TimeSpan.FromSeconds(20), cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Method responsible to queue new updated prices received from the CoinGecko API.
        /// </summary>
        /// <param name="coinMarketsUri">
        /// The coin markets uri.
        /// </param>
        /// <param name="coinIdList">
        /// The list with CoinIds.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task UpdateMarketPrices(string coinMarketsUri, IEnumerable<string> coinIdList, CancellationToken cancellationToken)
        {
            var coinMarketsDefinitions = await this.geckoService.GetMarketPricesFor(coinMarketsUri, coinIdList);
            var updatedCoinSignals =
                coinMarketsDefinitions?.Select(coinDef => this.mapper.Map<UpdatedCoinSignalMsg>(coinDef))
                ?? new List<UpdatedCoinSignalMsg>();
            foreach (var msg in updatedCoinSignals)
            {
                this.newMarketPrices.Add(msg, cancellationToken);
                Log.Information("The following result will be processed:\n "
                                + $"{nameof(msg.Symbol)}: {msg.Symbol}, "
                                + $"{nameof(msg.CurrentPrice)}: {msg.CurrentPrice}, "
                                + $"{nameof(msg.PriceChangePercentage24H)}: {msg.PriceChangePercentage24H}%");
            }
        }

        /// <summary>
        /// Method to consume the list of New Prices to inform the Clients via SignalR.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task ProcessSignalsQueue(CancellationToken cancellationToken)
        {
            foreach (var newPriceSignalMsg in this.newMarketPrices.GetConsumingEnumerable(cancellationToken))
            {
                await this.signalr.SendMessage(newPriceSignalMsg).ConfigureAwait(false);
            }
        }
    }
}

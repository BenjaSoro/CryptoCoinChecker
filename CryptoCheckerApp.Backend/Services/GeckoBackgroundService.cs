namespace CryptoCheckerApp.Backend.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using AutoMapper;

    using CryptoCheckerApp.Backend.GeckoApiDefinition.EndPoints;
    using CryptoCheckerApp.Backend.GeckoApiDefinition.Entities;
    using CryptoCheckerApp.Backend.Hubs;
    using CryptoCheckerApp.Domain.Models;

    using Microsoft.Extensions.Options;

    using Serilog;

    public class GeckoBackgroundService : IBackgroundService
    {
        private readonly GeckoApiDefinitionSettings geckoApiDefinitionSettings;

        private readonly ICoinService coinService;

        private readonly IGeckoService geckoService;

        private readonly IMapper mapper;

        private readonly ISignalr signalr;

        private readonly BlockingCollection<UpdatedCoinSignalMsg> newMarketPrices = new();
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

        private async Task ProcessSignalsQueue(CancellationToken cancellationToken)
        {
            foreach (var newPriceSignalMsg in this.newMarketPrices.GetConsumingEnumerable(cancellationToken))
            {
                await this.signalr.SendMessage(newPriceSignalMsg).ConfigureAwait(false);
            }
        }
    }
}

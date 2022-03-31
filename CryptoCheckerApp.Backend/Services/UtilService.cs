namespace CryptoCheckerApp.Backend.Services
{
    using System;
    using System.Collections.Generic;

    using CryptoCheckerApp.Backend.GeckoApiDefinition.Entities;
    using CryptoCheckerApp.Domain.Models;

    public class UtilService
    {
        public static IEnumerable<UpdatedCoinSignalMsg> MapFrom(IList<CoinMarketsDefinition> coinMarketsDefinitions)
        {
            if (coinMarketsDefinitions == null)
            {
                throw new ArgumentNullException(nameof(coinMarketsDefinitions));
            }

            if (coinMarketsDefinitions.Count == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(coinMarketsDefinitions));
            }

            foreach (var coinMarket in coinMarketsDefinitions)
            {
                var coinSignalMsg = new UpdatedCoinSignalMsg
                                        {
                                            Symbol = coinMarket.Symbol,
                                            CurrentPrice = coinMarket.CurrentPrice,
                                            PriceChangePercentage24 = coinMarket.PriceChangePercentage24H
                                        };
                yield return coinSignalMsg;
            }
        }
    }
}

namespace CryptoCheckerApp.Backend.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CryptoCheckerApp.Backend.Services;

    using NUnit.Framework;

    public class CoinServiceTests
    {
        public CoinService CoinService { get; set; }

        public IList<string> KnownCoinList { get; set; }

        [SetUp]
        public void Setup()
        {
            CoinService = new CoinService();

            KnownCoinList = new List<string>()
                                {
                                    "Bitcoin",
                                    "Ethereum",
                                    "Aave",
                                    "Algorand",
                                    "Cardano",
                                    "Chainlink",
                                    "Dogecoin",
                                    "Ethereum Classic",
                                    "Litecoin",
                                    "Shiba Inu",
                                    "Solana",
                                    "Tezos",
                                    "Uniswap",
                                };
        }

        [Test]
        public void IsTheSameAmountSupported()
        {
            var listAvailable = CoinService.GetAvailableCoins();
            Assert.True(listAvailable.Count == KnownCoinList.Count);
        }

        [Test]
        public void AreTheSameCoinNamesSupported()
        {
            var r = CoinService.GetNameListAvailableCoins().All(coin => KnownCoinList.Any(knownCoin => 
                string.Compare(knownCoin, coin, StringComparison.InvariantCultureIgnoreCase) == 0));
            Assert.True(r);
        }
    }
}
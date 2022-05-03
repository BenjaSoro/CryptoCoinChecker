namespace CryptoCheckerApp.Backend.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CryptoCheckerApp.Domain.Entities;

    /// <inheritdoc />
    public class CoinService : ICoinService
    {
        /// <summary>
        /// Defined list of available Coins supported by the Clients.
        /// </summary>
        private readonly IList<Coin> availableCoinsList = new List<Coin>
                                                              {
                                                                  new () { Id = Guid.NewGuid(), Name = "Bitcoin", Symbol = "BTC" },
                                                                  new () { Id = Guid.NewGuid(), Name = "Ethereum", Symbol = "ETH" },
                                                                  new () { Id = Guid.NewGuid(), Name = "Aave", Symbol = "AAVE" },
                                                                  new () { Id = Guid.NewGuid(), Name = "Algorand", Symbol = "ALGO" },
                                                                  new () { Id = Guid.NewGuid(), Name = "Cardano", Symbol = "ADA" },
                                                                  new () { Id = Guid.NewGuid(), Name = "Chainlink", Symbol = "LINK" },
                                                                  new () { Id = Guid.NewGuid(), Name = "Dogecoin", Symbol = "DOGE" },
                                                                  new () { Id = Guid.NewGuid(), Name = "Ethereum Classic", Symbol = "ETC" },
                                                                  new () { Id = Guid.NewGuid(), Name = "Litecoin", Symbol = "LTC" },
                                                                  new () { Id = Guid.NewGuid(), Name = "Shiba Inu", Symbol = "SHIB" },
                                                                  new () { Id = Guid.NewGuid(), Name = "Solana", Symbol = "SOL" },
                                                                  new () { Id = Guid.NewGuid(), Name = "Tezos", Symbol = "XTZ" },
                                                                  new () { Id = Guid.NewGuid(), Name = "Uniswap", Symbol = "UNI" },
                                                              };

        /// <inheritdoc />
        public IList<Coin> GetAvailableCoins()
        {
            return this.availableCoinsList;
        }

        /// <inheritdoc />
        public IEnumerable<string> GetNameListAvailableCoins()
        {
            return this.availableCoinsList.Select(coin => coin.Name.ToLowerInvariant());
        }
    }
}

namespace CryptoCheckerApp.Backend.Services
{
    using System.Collections.Generic;

    using CryptoCheckerApp.Domain.Entities;

    public interface ICoinService
    {
        IList<Coin> GetAvailableCoins();

        IEnumerable<string> GetNameListAvailableCoins();
    }
}

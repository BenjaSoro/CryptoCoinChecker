namespace CryptoCheckerApp.Backend.Services
{
    using System.Collections.Generic;

    using CryptoCheckerApp.Domain.Entities;

    /// <summary>
    /// Interface which defines the methods to interact with available Coins.
    /// </summary>
    public interface ICoinService
    {
        /// <summary>
        /// Method to get the available Coins.
        /// </summary>
        /// <returns>
        /// The <see cref="IList"/> list of available coins.
        /// </returns>
        IList<Coin> GetAvailableCoins();

        /// <summary>
        /// Gets the Name of available Coins.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/> list of name of available coins.
        /// </returns>
        IEnumerable<string> GetNameListAvailableCoins();
    }
}

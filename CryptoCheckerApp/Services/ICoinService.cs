namespace CryptoChecker.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CryptoChecker.Models;

    /// <summary>
    /// The Coin Service interface.
    /// </summary>
    public interface ICoinService
    {
        /// <summary>
        /// Method to return the list of Coin items.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/> with a list of available Coin Item.
        /// </returns>
        Task<IList<CoinItem>> GetCoins();

        /// <summary>
        /// Method to return a Coin Item filtered by name.
        /// </summary>
        /// <param name="name">
        /// The coin name.
        /// </param>
        /// <returns>
        /// The filtered list of Coin item.
        /// </returns>
        IList<CoinItem> FilterByName(string name);
    }
}
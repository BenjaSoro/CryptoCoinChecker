namespace CryptoCheckerApp.Backend.Hubs
{
    using System.Threading.Tasks;

    using CryptoCheckerApp.Domain.Models;

    /// <summary>
    /// The SignalR interface definition.
    /// </summary>
    public interface ISignalr
    {
        /// <summary>
        /// Method being used to inform about the new update prices to the clients.
        /// </summary>
        /// <param name="message">
        /// The <see cref="UpdatedCoinSignalMsg"/> message.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task SendMessage(UpdatedCoinSignalMsg message);
    }
}

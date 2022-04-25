namespace CryptoChecker.Services
{
    using System;
    using System.Threading.Tasks;

    using global::CryptoCheckerApp.Domain.Models;

    using Microsoft.AspNetCore.SignalR.Client;

    /// <summary>
    /// The SignalR Service interface.
    /// </summary>
    public interface ISignalRService
    {
        /// <summary>
        /// The connect method to start the connection with the backend Hub.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task Connect();

        /// <summary>
        /// The disconnect method to dispose the current Hub connection.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task Disconnect();

        /// <summary>
        /// Method to add the action parameter on the invocation of the method message received.
        /// </summary>
        /// <param name="doAction">
        /// The action to invoke after message is received.
        /// </param>
        void OnReceiveMessage(Action<UpdatedCoinSignalMsg> doAction);

        /// <summary>
        /// Method to get the current Hub connection.
        /// </summary>
        /// <returns>
        /// The <see cref="HubConnection"/>.
        /// </returns>
        HubConnection HubConnection();
    }
}
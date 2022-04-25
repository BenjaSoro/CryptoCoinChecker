namespace CryptoChecker.Services
{
    using System;
    using System.Threading.Tasks;

    using CryptoChecker.Utils;

    using CryptoCheckerApp.Domain.Models;

    using Microsoft.AspNetCore.SignalR.Client;

    /// <summary>
    /// Class responsible for SignalR logic.
    /// </summary>
    public class CoinHubService : ISignalRService, IDisposable
    {
        /// <summary>
        /// SignalR backend hub url.
        /// </summary>
        private const string SignalRHubEndPoint = BackendConstants.RootBackendUrl + "CheckerHub";

        /// <summary>
        /// The hub connection.
        /// </summary>
        private readonly HubConnection hubConnection;

        /// <summary>
        /// The reconnection time spans.
        /// </summary>
        private readonly TimeSpan[] reconnectionTimeSpans =
            {
                TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(40), TimeSpan.FromMinutes(1)
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="CoinHubService"/> class.
        /// </summary>
        public CoinHubService()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(SignalRHubEndPoint)
                .WithAutomaticReconnect(reconnectionTimeSpans)
                .Build();
        }

        /// <summary>
        /// The dispose method to disconnect from the Hub.
        /// </summary>
        public void Dispose()
        {
            _ = Disconnect();
        }

        /// <inheritdoc />
        public async Task Connect()
        {
            if (hubConnection.State == HubConnectionState.Connected || hubConnection.State == HubConnectionState.Connecting)
            {
                return;
            }

            await hubConnection.StartAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task Disconnect()
        {
            await hubConnection.DisposeAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public void OnReceiveMessage(Action<UpdatedCoinSignalMsg> doAction)
        {
            hubConnection.On("ReceiveMessage", doAction);
        }

        /// <inheritdoc />
        HubConnection ISignalRService.HubConnection()
        {
            return hubConnection;
        }
    }
}
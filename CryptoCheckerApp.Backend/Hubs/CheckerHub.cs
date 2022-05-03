namespace CryptoCheckerApp.Backend.Hubs
{
    using System;
    using System.Threading.Tasks;

    using CryptoCheckerApp.Domain.Models;

    using Microsoft.AspNetCore.SignalR;

    /// <summary>
    /// Class which represents the SignalR Hub definition.
    /// </summary>
    public class CheckerHub : Hub, ISignalr
    {
        /// <summary>
        /// The Hub Context.
        /// </summary>
        private readonly IHubContext<CheckerHub> context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckerHub"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public CheckerHub(IHubContext<CheckerHub> context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc />
        public Task SendMessage(UpdatedCoinSignalMsg message)
        {
            return this.context.Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}

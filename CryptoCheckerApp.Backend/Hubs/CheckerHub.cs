namespace CryptoCheckerApp.Backend.Hubs
{
    using System;
    using System.Threading.Tasks;

    using CryptoCheckerApp.Domain.Models;

    using Microsoft.AspNetCore.SignalR;

    public class CheckerHub : Hub, ISignalr
    {
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

        public Task SendMessage(UpdatedCoinSignalMsg message)
        {
            return this.context.Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}

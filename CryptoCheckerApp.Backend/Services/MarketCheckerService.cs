namespace CryptoCheckerApp.Backend.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Class which represents the implementation of a BackgroundService instance for the Market Checker.
    /// </summary>
    public class MarketCheckerService : BackgroundService, IMarketCheckerService
    {
        /// <summary>
        /// Injected instance of Background service.
        /// </summary>
        private readonly IBackgroundService backgroundService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketCheckerService"/> class.
        /// </summary>
        /// <param name="backgroundService">
        /// Injected instance of Background service.
        /// </param>
        public MarketCheckerService(IBackgroundService backgroundService)
        {
            this.backgroundService = backgroundService ?? throw new ArgumentNullException(nameof(backgroundService));
        }

        /// <summary>
        /// Method responsible to execute a task as background.
        /// </summary>
        /// <param name="stoppingToken">
        /// The stopping token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => this.backgroundService.DoWorkAsync(stoppingToken), stoppingToken);
        }
    }
}

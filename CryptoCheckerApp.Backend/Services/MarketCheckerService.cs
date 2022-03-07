namespace CryptoCheckerApp.Backend.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Hosting;

    public class MarketCheckerService : BackgroundService, IMarketCheckerService
    {
        private readonly IBackgroundService backgroundService;

        public MarketCheckerService(IBackgroundService backgroundService)
        {
            this.backgroundService = backgroundService ?? throw new ArgumentNullException(nameof(backgroundService));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => this.backgroundService.DoWorkAsync(stoppingToken), stoppingToken);
        }
    }
}

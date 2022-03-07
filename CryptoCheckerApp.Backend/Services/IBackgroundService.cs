namespace CryptoCheckerApp.Backend.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IBackgroundService
    {
        Task DoWorkAsync(CancellationToken cancellationToken);
    }
}

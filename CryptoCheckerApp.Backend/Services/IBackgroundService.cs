namespace CryptoCheckerApp.Backend.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface which represents the definitions of background tasks.
    /// </summary>
    public interface IBackgroundService
    {
        /// <summary>
        /// Method to be called as background task.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task DoWorkAsync(CancellationToken cancellationToken);
    }
}

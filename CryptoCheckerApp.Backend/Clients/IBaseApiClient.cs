namespace CryptoCheckerApp.Backend.Clients
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface which represents the Base API Client definition methods.
    /// </summary>
    public interface IBaseApiClient
    {
        /// <summary>
        /// GET method responsible to request a resource and return it parsed in the type provided.
        /// </summary>
        /// <param name="resourceUri">
        /// The resource uri.
        /// </param>
        /// <typeparam name="T">
        /// The Type of the resource to be parsed.
        /// </typeparam>
        /// <returns>
        /// A <see cref="Task"/> task with the resourced type parsed.
        /// </returns>
        Task<T> GetAsync<T>(Uri resourceUri);
    }
}

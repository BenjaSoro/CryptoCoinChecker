namespace CryptoCheckerApp.Backend.Clients
{
    using System;
    using System.Threading.Tasks;

    public interface IBaseApiClient
    {
        Task<T> GetAsync<T>(Uri resourceUri);
    }
}

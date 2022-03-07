namespace CryptoCheckerApp.Backend.Hubs
{
    using System.Threading.Tasks;

    using CryptoCheckerApp.Domain.Models;

    public interface ISignalr
    {
        Task SendMessage(UpdateCoinSignalMsg message);
    }
}

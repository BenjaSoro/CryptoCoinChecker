namespace CryptoChecker.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using CryptoChecker.Models;
    using CryptoChecker.Services;

    using CryptoCheckerApp.Domain.Models;

    using Microsoft.AspNetCore.SignalR.Client;

    using Xamarin.Forms;

    /// <summary>
    /// Class which represents the View model for the Markets page.
    /// </summary>
    public class MarketsViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarketsViewModel"/> class.
        /// </summary>
        public MarketsViewModel()
        {
            Title = "Crypto Markets";
            CoinItems = new ObservableCollection<CoinItem>();
            LoadingMessageLabel = new LoadingMessageLabel();
            SignalRConnectionLabel = new SignalRConnectionLabel();
            _ = GetCoinItems();
            SearchResultLabel = new SearchResultLabel();
        }

        /// <summary>
        /// Gets or sets the label for showing the search results.
        /// </summary>
        public SearchResultLabel SearchResultLabel { get; set; }

        /// <summary>
        /// Gets or sets the label for loading the available coins.
        /// </summary>
        public LoadingMessageLabel LoadingMessageLabel { get; set; }

        /// <summary>
        /// Gets or sets the label for establishing the connection with the Hub.
        /// </summary>
        public SignalRConnectionLabel SignalRConnectionLabel { get; set; }

        /// <summary>
        /// Gets the collection of the Coin items.
        /// </summary>
        public ObservableCollection<CoinItem> CoinItems { get; }

        /// <summary>
        /// Command to refresh the list of Coins in case of error.
        /// </summary>
        public ICommand RefreshCoins =>
            new Command(
                () =>
                    {
                        IsThereAnError = false;
                        LoadingMessageLabel.IsShowing = true;
                        _ = GetCoinItems();
                    });

        /// <summary>
        /// Command to perform a search over the list of available coins.
        /// </summary>
        public ICommand PerformSearchCommand =>
            new Command<string>(
                query =>
                    {
                        SearchResultLabel.IsShowing = false;
                        var queryTrimmed = query.Trim();
                        if (string.IsNullOrEmpty(queryTrimmed))
                        {
                            _ = CoinService.GetCoins().ContinueWith(
                                async task =>
                                    {
                                        var cItems = await task.ConfigureAwait(false);
                                        FillCollectionWith(CoinItems, cItems);
                                    });
                        }
                        else
                        {
                            var filteredByNameList = GetCoinByName(queryTrimmed);
                            FillCollectionWith(CoinItems, filteredByNameList);
                            SearchResultLabel.SetMessageBaseOnResults(CoinItems.Count, queryTrimmed);
                            SearchResultLabel.IsShowing = true;
                        }
                    });

        /// <summary>
        /// The coin service instance.
        /// </summary>
        private static ICoinService CoinService => DependencyService.Get<ICoinService>();

        /// <summary>
        /// The coin hub service instance.
        /// </summary>
        private static ISignalRService CoinHubService => DependencyService.Get<ISignalRService>();

        /// <summary>
        /// Method to fill a collection with new values.
        /// </summary>
        /// <param name="collection">
        /// The source collection.
        /// </param>
        /// <param name="newCollection">
        /// The new collection.
        /// </param>
        private static void FillCollectionWith(
            ICollection<CoinItem> collection,
            IEnumerable<CoinItem> newCollection)
        {
            collection?.Clear();
            foreach (var coinItem in newCollection)
            {
                collection?.Add(coinItem);
            }
        }

        /// <summary>
        /// Method to get the list of coins filtered by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The list of filtered coins.
        /// </returns>
        private static IEnumerable<CoinItem> GetCoinByName(string name)
        {
            return CoinService.FilterByName(name);
        }

        /// <summary>
        /// Method to get the available coins and fill the observable collection with results.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task GetCoinItems()
        {
            try
            {
                var serviceCoins = await CoinService.GetCoins().ConfigureAwait(false);
                FillCollectionWith(CoinItems, serviceCoins);
                LoadingMessageLabel.IsDone = true;
                LoadingMessageLabel.IsShowing = false;
                if (CoinHubService.HubConnection().State == HubConnectionState.Disconnected)
                {
                    _ = SetupSignal();
                }
            }
            catch
            {
                IsThereAnError = true;
                LoadingMessageLabel.IsShowing = false;
                throw;
            }
        }

        /// <summary>
        /// Method to establish the synchronization with SignalR on the backend.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task SetupSignal()
        {
            try
            {
                SignalRConnectionLabel.IsShowing = true;
                await CoinHubService.Connect().ConfigureAwait(false);
                if (CoinHubService.HubConnection().State != HubConnectionState.Disconnected)
                {
                    SignalRConnectionLabel.IsShowing = false;
                }

                CoinHubService.OnReceiveMessage(GetMessage);
            }
            catch
            {
                await SetupSignal().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Method as an action attached to the OnReceiveMessage method of the Hub for new messages.
        /// </summary>
        /// <param name="message">
        /// The message received from the backend.
        /// </param>
        private void GetMessage(UpdatedCoinSignalMsg message)
        {
            var coinVm = CoinItems.FirstOrDefault(item => string.Equals(item.Coin.Symbol, message.Symbol, StringComparison.InvariantCultureIgnoreCase));
            if (coinVm != null)
            {
                coinVm.CurrentPrice = message.CurrentPrice ?? 0;
                coinVm.ChangedIn24Hours = message.PriceChangePercentage24H;
                coinVm.PercentBgColor = coinVm.ChangedIn24Hours == 0 ? Color.Gray : coinVm.ChangedIn24Hours > 0 ? Color.GreenYellow : Color.Red;
            }
        }
    }
}
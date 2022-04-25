namespace CryptoChecker.ViewModels
{
    using System.Windows.Input;

    using Xamarin.Essentials;
    using Xamarin.Forms;

    /// <summary>
    /// Class which represents the View model for the About page.
    /// </summary>
    public class AboutViewModel : BaseViewModel
    {
        /// <summary>
        /// The Github project repository.
        /// </summary>
        public string ProjectRepository => "https://github.com/BenjaSoro/CryptoCoinChecker";

        /// <summary>
        /// The CoinGecko url API end point.
        /// </summary>
        public string CoinGeckoUrl => "https://www.coingecko.com/en/api";

        /// <summary>
        /// Gets or sets the app logo.
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// Gets the current app version.
        /// </summary>
        public string CurrentVersion { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutViewModel"/> class.
        /// </summary>
        public AboutViewModel()
        {
            this.Title = "About";
            this.Logo = "app-logo.png";
            this.CurrentVersion = VersionTracking.CurrentVersion;
        }

        /// <summary>
        /// Command to open an url in the Browser.
        /// </summary>
        public ICommand OpenWebCommand =>
        new Command<string>(
            async (index) =>
                {
                    var urlToOpen = this.ProjectRepository;
                    switch (index)
                    {
                        case "1":
                            urlToOpen = CoinGeckoUrl;
                            break;
                    }

                    await Browser.OpenAsync(urlToOpen, BrowserLaunchMode.External).ConfigureAwait(false);
                });
    }
}
namespace CryptoChecker
{
    using System.Net.Http;

    using CryptoChecker.Services;

    using Xamarin.Forms;

    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            Xamarin.Essentials.VersionTracking.Track();
            DependencyService.Register<HttpClient>();
            DependencyService.Register<CoinService>();
            DependencyService.Register<CoinHubService>();
            this.MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

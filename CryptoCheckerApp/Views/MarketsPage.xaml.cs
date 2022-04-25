namespace CryptoChecker.Views
{
    using CryptoChecker.ViewModels;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MarketsPage : ContentPage
    {
        public MarketsPage()
        {
            InitializeComponent();
        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            (this.BindingContext as MarketsViewModel)?.PerformSearchCommand.Execute(e.NewTextValue);
        }
    }
}
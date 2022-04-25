namespace CryptoChecker.Models
{
    using CryptoChecker.Models.BaseModels;

    using CryptoCheckerApp.Domain.Entities;
    using Xamarin.Forms;

    /// <summary>
    /// Class which presents an item in the list of available coins.
    /// </summary>
    public class CoinItem : BaseModel
    {
        /// <summary>
        /// The current decimal price.
        /// </summary>
        private decimal currentPrice;

        /// <summary>
        /// Percentage changed in the last 24 hours.
        /// </summary>
        private double? changedIn24Hours;

        /// <summary>
        /// Background color to show weather the percentage was positive or negative.
        /// </summary>
        private Color percentBgColor;

        /// <summary>
        /// Gets or sets the Coin model.
        /// </summary>
        public Coin Coin { get; set; }

        /// <summary>
        /// Gets or sets the logo for the coin path.
        /// </summary>
        public string LogoPath { get; set; }

        /// <summary>
        /// Gets or sets the current price.
        /// </summary>
        public decimal CurrentPrice
        {
            get => this.currentPrice;
            set => this.SetProperty(ref this.currentPrice, value);
        }

        /// <summary>
        /// Gets or sets the price changed in 24 hours.
        /// </summary>
        public double? ChangedIn24Hours
        {
            get => this.changedIn24Hours;
            set => this.SetProperty(ref this.changedIn24Hours, value);
        }

        /// <summary>
        /// Gets or sets the percent background color.
        /// </summary>
        public Color PercentBgColor
        {
            get => this.percentBgColor;
            set => this.SetProperty(ref this.percentBgColor, value);
        }
    }
}

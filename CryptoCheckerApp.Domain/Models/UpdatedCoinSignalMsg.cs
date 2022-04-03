namespace CryptoCheckerApp.Domain.Models
{
    public class UpdatedCoinSignalMsg
    {
        public string Symbol { get; set; }

        public decimal? CurrentPrice { get; set; }

        public double? PriceChangePercentage24H { get; set; }
    }
}

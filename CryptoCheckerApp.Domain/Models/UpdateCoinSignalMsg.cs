﻿namespace CryptoCheckerApp.Domain.Models
{
    public class UpdateCoinSignalMsg
    {
        public string Symbol { get; set; }

        public decimal? CurrentPrice { get; set; }

        public double? PriceChangePercentage24 { get; set; }
    }
}
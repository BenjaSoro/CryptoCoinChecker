namespace CryptoCheckerApp.Domain.Entities
{
    using System;

    public class Coin
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Symbol { get; set; }
    }
}

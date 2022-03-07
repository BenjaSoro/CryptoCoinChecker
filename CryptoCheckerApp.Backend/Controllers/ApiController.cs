namespace CryptoCheckerApp.Backend.Controllers
{
    using System;
    using System.Collections.Generic;

    using CryptoCheckerApp.Backend.Services;
    using CryptoCheckerApp.Domain.Entities;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ICoinService coinService;

        public ApiController(ICoinService coinService)
        {
            this.coinService = coinService ?? throw new ArgumentNullException(nameof(coinService));
        }

        [HttpGet]
        [Route("GetAvailableCoins")]
        public IList<Coin> GetAvailableCoins()
        {
            return this.coinService.GetAvailableCoins();
        }
    }
}

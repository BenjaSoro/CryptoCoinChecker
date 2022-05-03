namespace CryptoCheckerApp.Backend.Controllers
{
    using System;
    using System.Collections.Generic;

    using CryptoCheckerApp.Backend.Services;
    using CryptoCheckerApp.Domain.Entities;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Base API controller to manage the request from the Client.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        /// <summary>
        /// The injected coin service.
        /// </summary>
        private readonly ICoinService coinService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiController"/> class.
        /// </summary>
        /// <param name="coinService">
        /// The coin service.
        /// </param>
        public ApiController(ICoinService coinService)
        {
            this.coinService = coinService ?? throw new ArgumentNullException(nameof(coinService));
        }

        /// <summary>
        /// Method which gets the list of available Coins provided by Backend.
        /// </summary>
        /// <returns>
        /// The list of available Coins.
        /// </returns>
        [HttpGet]
        [Route("GetAvailableCoins")]
        public IList<Coin> GetAvailableCoins()
        {
            return this.coinService.GetAvailableCoins();
        }
    }
}

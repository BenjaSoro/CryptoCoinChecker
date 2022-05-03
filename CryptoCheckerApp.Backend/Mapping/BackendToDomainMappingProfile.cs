namespace CryptoCheckerApp.Backend.Mapping
{
    using AutoMapper;

    using CryptoCheckerApp.Backend.GeckoApiDefinition.Entities;
    using CryptoCheckerApp.Domain.Models;

    /// <summary>
    /// Class which represents the Profile for AutoMapper.
    /// </summary>
    public class BackendToDomainMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackendToDomainMappingProfile"/> class.
        /// </summary>
        public BackendToDomainMappingProfile()
        {
            this.CreateMap<CoinMarketsDefinition, UpdatedCoinSignalMsg>();
        }
    }
}
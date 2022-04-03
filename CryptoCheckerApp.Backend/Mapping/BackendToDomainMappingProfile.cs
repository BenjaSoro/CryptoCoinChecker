namespace CryptoCheckerApp.Backend.Mapping
{
    using AutoMapper;

    using CryptoCheckerApp.Backend.GeckoApiDefinition.Entities;
    using CryptoCheckerApp.Domain.Models;

    public class BackendToDomainMappingProfile : Profile
    {
        public BackendToDomainMappingProfile()
        {
            this.CreateMap<CoinMarketsDefinition, UpdatedCoinSignalMsg>();
        }
    }
}
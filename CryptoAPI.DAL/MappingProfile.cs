using AutoMapper;
using CryptoAPI.Core.Models;
using CryptoAPI.DAL.Entities;
using User = CryptoAPI.DAL.Entities.User;

namespace CryptoAPI.DAL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Core.Models.User, DAL.Entities.User>().ReverseMap();
            CreateMap<Core.Models.PortfolioItem, Entities.PortfolioItem>();
            CreateMap<Entities.PortfolioItem, Core.Models.PortfolioItem>();
            CreateMap<Entities.Portfolio, Core.Models.Portfolio>();
            CreateMap<Core.Models.Portfolio, Entities.Portfolio>();
            CreateMap<Core.Models.PortfolioNote, Entities.PortfolioNote>();
            CreateMap<Entities.PortfolioNote, Core.Models.PortfolioNote>();
        }
    }
}
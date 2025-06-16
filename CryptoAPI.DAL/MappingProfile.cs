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
            CreateMap<Core.Models.PortfolioItem, Entities.PortfolioItem>().ReverseMap();
            CreateMap<Entities.Portfolio, Core.Models.Portfolio>().ReverseMap();
            CreateMap<Core.Models.PortfolioNote, Entities.PortfolioNote>().ReverseMap();
        }
    }
}
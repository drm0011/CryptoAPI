using AutoMapper;
using CryptoAPI.Core.DTOs;
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
            CreateMap<PortfolioItemDto, PortfolioItem>();
            CreateMap<PortfolioItem, PortfolioItemDto>();
        }
    }
}
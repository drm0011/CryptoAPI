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
        }
    }
}
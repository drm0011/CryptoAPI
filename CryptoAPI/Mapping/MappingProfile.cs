using AutoMapper;
using CryptoAPI.DTOs;
using CryptoAPI.Core.Models;
using System.Runtime.InteropServices;

namespace CryptoAPI.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<CryptoAPI.DTOs.UserDto, User>();
            CreateMap<CryptoAPI.DTOs.LoginDto, User>();
        }
    }
}

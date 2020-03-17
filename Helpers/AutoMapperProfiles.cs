using System.Linq;
using AutoMapper;
using DatingApp.Api.DTOS;
using DatingApp.Api.Models;

namespace DatingApp.Api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User,UserForListDto>()
            .ForMember(dec => dec.PhotoUrl, 
            opt=> opt.MapFrom(src=> src.Photos.FirstOrDefault(p=> p.IsMain==true).Url ))
            .ForMember(dec => dec.Age, 
            opt=> opt.MapFrom(src=> src.DateOfBirth.CalculateAge()));

            CreateMap<User, UserForDetailedDto>()
            .ForMember(dec => dec.PhotoUrl, 
            opt=> opt.MapFrom(src=> src.Photos.FirstOrDefault(p=> p.IsMain==true).Url ))
            .ForMember(dec => dec.Age, 
            opt=> opt.MapFrom(src=> src.DateOfBirth.CalculateAge()));
            

            CreateMap<Photo,PhotoForDetailedDto>();
            
        }
    }
}
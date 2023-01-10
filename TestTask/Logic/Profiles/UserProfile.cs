using AutoMapper;
using Dal.Entities;
using Logic.Models;

namespace Logic.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterRequestModel, User>()
            .ForMember(dst => dst.Id, opt => opt.Ignore())
            .ForMember(dst => dst.Email, opt => opt.MapFrom(src=> src.Email))
            .ForMember(dst => dst.Phone, opt => opt.MapFrom(src=> src.Phone))
            .ForMember(dst => dst.Password, opt => opt.MapFrom(src=> src.Password))
            .ForMember(dst => dst.FIO, opt => opt.MapFrom(src=> src.FIO))
            .ForMember(dst => dst.LastLogin, opt => opt.MapFrom(src=> DateTime.UtcNow))

            ;
    }
}
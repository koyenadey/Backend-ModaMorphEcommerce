using AutoMapper;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.Shared.Profiles;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.ValueObject;

namespace ECommWeb.Business.src.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Profile for CreateDto
        //CreateMap<UserCreateDto, User>();
        CreateMap<UserCreateDto, User>()
        .ForMember(
            dest => dest.Id,
            opt => opt.MapFrom(src => Guid.NewGuid())
        )
        .ForMember(
            dest => dest.UserName,
            opt => opt.MapFrom(src => src.UserName)
        )
        .ForMember(
            dest => dest.Email,
            opt => opt.MapFrom(src => src.Email)
        )
        .ForMember(
            dest => dest.Role,
            opt => opt.MapFrom(src => Role.Customer)
        )
        .ForMember(
            dest => dest.Password,
            opt => opt.MapFrom(src => src.Password)
        )
        .ForMember(
            dest => dest.CreatedAt,
            opt => opt.MapFrom(src => DateTime.Now)
        )
        .ForMember(
            dest => dest.UpdatedAt,
            opt => opt.MapFrom(src => DateTime.Now)
        )
        .ForMember(
            dest => dest.Avatar,
            opt => opt.MapFrom(src => src.Avatar)
        );

        // Profile for ReadDto

        CreateMap<User, UserReadDto>()
        .ForMember(
            dest => dest.Id,
            opt => opt.MapFrom(src => src.Id)
        )
        .ForMember(
            dest => dest.UserName,
            opt => opt.MapFrom(src => src.UserName)
        )
        .ForMember(
            dest => dest.Email,
            opt => opt.MapFrom(src => src.Email)
        )
        .ForMember(
            dest => dest.Role,
            opt => opt.MapFrom(src => src.Role)
        )
        .ForMember(
            dest => dest.Avatar,
            opt => opt.MapFrom(src => src.Avatar)
        );
    }

}

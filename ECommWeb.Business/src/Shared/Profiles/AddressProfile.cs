using AutoMapper;
using ECommWeb.Business.src.DTO;
using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.Shared.Profiles;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        //Profile for CreateAddressDto
        CreateMap<AddressCreateDto, Address>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => Guid.NewGuid())
            )
            .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId)
            )
            .ForMember(
                dest => dest.AddressLine,
                opt => opt.MapFrom(src => src.AddressLine ?? string.Empty)
            )
            .ForMember(
                dest => dest.City,
                opt => opt.MapFrom(src => src.City)
            )
            .ForMember(
                dest => dest.Country,
                opt => opt.MapFrom(src => src.Country)
            )
            .ForMember(
                dest => dest.Postcode,
                opt => opt.MapFrom(src => src.Postcode)
            )
            .ForMember(
                dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.PhoneNumber)
            )
            .ForMember(
                dest => dest.Landmark,
                opt => opt.MapFrom(src => src.Landmark ?? string.Empty)
            )
            .ForMember(
                dest => dest.CreatedAt,
                opt => opt.MapFrom(src => DateTime.Now)
            ).ForMember(
                dest => dest.UpdatedAt,
                opt => opt.MapFrom(src => DateTime.Now)
            );

        CreateMap<User, AddressReadDtoWithUser>()
            .ForMember(
                dest => dest.UserName,
                opt => opt.MapFrom(src => src.UserName)
            )
            .ForMember(
                dest => dest.Email,
                opt => opt.MapFrom(src => src.Email)
            );

        //Profile for ReadAddressDto
        CreateMap<Address, AddressReadDto>()
            .ForMember(
                dest => dest.User,
                opt => opt.MapFrom(src => src.User)
            )
            .ForMember(
                dest => dest.AddressLine,
                opt => opt.MapFrom(src => src.AddressLine ?? "No street address")
            )
            .ForMember(
                dest => dest.Street,
                opt => opt.MapFrom(src => src.Street)
            )
            .ForMember(
                dest => dest.City,
                opt => opt.MapFrom(src => src.City)
            )
            .ForMember(
                dest => dest.Postcode,
                opt => opt.MapFrom(src => src.Postcode)
            )
            .ForMember(
                dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.PhoneNumber)
            )
            .ForMember(
                dest => dest.Landmark,
                opt => opt.MapFrom(src => src.Landmark ?? "No landmark listed")
            );

        CreateMap<User, AddressReadDtoWithUser>()
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
            );
    }


}



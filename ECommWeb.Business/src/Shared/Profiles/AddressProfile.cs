using AutoMapper;
using ECommWeb.Business.src.DTO;
using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.Shared.Profiles;

public class AddressProfile : Profile
{
    // Define a method to handle the mapping logic for ProductName
    // public string MapUserName(Address src)
    // {
    //     Console.WriteLine($"User: {src.User.UserName}");
    //     return src.User.UserName;
    // }
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

        //Profile for ReadAddressDto
        CreateMap<Address, AddressReadDto>()
            .IncludeMembers(src => src.User)
            .ForMember(
                dest => dest.Username,
               opt => opt.MapFrom(src => src.User.UserName)
            )
            .ForMember(
                dest => dest.EmailAddress,
                opt => opt.MapFrom(src => src.User.Email)
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
            )
            .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId)
            );


        CreateMap<User, AddressReadDto>()
        .ForMember(
            dest => dest.Username,
            opt => opt.MapFrom(src => src.UserName)
        )
        .ForMember(
            dest => dest.EmailAddress,
             opt => opt.MapFrom(src => src.Email)
        )
        .ForMember(
            dest => dest.AddressLine,
             opt => opt.MapFrom(src => src.Addresses.FirstOrDefault(a => a.Id == src.DefaultAddressId).AddressLine)
        ) // Example
        .ForMember(
            dest => dest.Street,
            opt => opt.MapFrom(src => "User's Street")
        ) // Example
        .ForMember(
            dest => dest.City,
            opt => opt.MapFrom(src => "User's City")
        ) // Example
        .ForMember(
            dest => dest.Country,
            opt => opt.MapFrom(src => "User's Country")
        ) // Example
        .ForMember(
            dest => dest.Postcode,
            opt => opt.MapFrom(src => "User's Postcode")
        ) // Example
        .ForMember(
            dest => dest.PhoneNumber,
            opt => opt.MapFrom(src => "User's Phone Number")
        ) // Example
        .ForMember(
            dest => dest.Landmark,
            opt => opt.MapFrom(src => "User's Landmark")
        ); // Example
    }


}



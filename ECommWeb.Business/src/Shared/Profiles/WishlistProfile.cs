using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommWeb.Business.src.DTO;
using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.Shared.Profiles;

public class WishlistProfile : Profile
{
    public WishlistProfile()
    {
        CreateMap<Wishlist, WishlistReadDto>()
            .ForMember(
                dest => dest.User,
                opt => opt.MapFrom(src => src.User)
            )
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            )
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name)
            )
            .ForMember(
                dest => dest.WishlistItems,
                opt => opt.MapFrom(src => src.WishlistItems)
            );

        CreateMap<WishlistItem, WishlistReadItemDto>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            )
            .ForPath(
                dest => dest.Product,
                opt => opt.MapFrom(src => src.Product)
            );

        CreateMap<User, UserReadDto>();

        CreateMap<Product, ProductReadDTO>();

        CreateMap<WishlistCreateDto, Wishlist>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name)
            ).ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => Guid.NewGuid())
            ).ForMember(
                dest => dest.WishlistItems,
                opt => opt.MapFrom(src => new List<WishlistItem>())
            );
    }
}

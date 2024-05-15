using AutoMapper;
using ECommWeb.Business.src.DTO;
using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.Shared.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryCreateDTO, Category>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => Guid.NewGuid())
            )
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name)
            )
            .ForMember(
                dest => dest.Image,
                opt => opt.MapFrom(src => src.Image)
            )
             .ForMember(
                dest => dest.Parent_id,
                opt => opt.MapFrom(src => src.ParentId)
            );

        CreateMap<Category, CategoryReadDTO>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            )
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name)
            )
            .ForMember(
                dest => dest.Image,
                opt => opt.MapFrom(src => src.Image)
            );
    }
}

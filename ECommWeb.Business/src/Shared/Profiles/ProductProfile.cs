using AutoMapper;
using ECommWeb.Business.src.DTO;
using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.Shared.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {

        // Profile for Reading Products
        CreateMap<Product, ProductReadDTO>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id)
                )
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name)
                )
                .ForMember(
                    dest => dest.Price,
                     opt => opt.MapFrom(src => src.Price)
                )
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description)
                )
                .ForMember(
                    dest => dest.Inventory,
                    opt => opt.MapFrom(src => src.Inventory)
                )
                .ForMember(
                    dest => dest.Weight,
                    opt => opt.MapFrom(src => src.Weight)
                )
                .ForMember(
                    dest => dest.Category,
                    opt => opt.MapFrom(
                        src => new CategoryReadDTO
                        {
                            Id = src.Category.Id,
                            Name = src.Category.Name,
                            Image = src.Category.Image,
                            CreatedAt = src.Category.CreatedAt,
                            UpdatedAt = src.Category.UpdatedAt,

                        }
                    )
                )
                .ForMember(
                    dest => dest.Images,
                    opt => opt.MapFrom(
                        src => src.Images.Where(i => i.ProductId == src.Id).Select(
                            i => new ProductImageReadDTO
                            {
                                ProductId = i.ProductId,
                                ImageUrl = i.ProductImageUrl
                            }
                        )
                    )
                )
                .ReverseMap();


        //Profile for ProductCreateDto
        CreateMap<ProductCreateDTO, Product>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => Guid.NewGuid())
            )
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name)
            )
            .ForMember(
                dest => dest.Price,
                opt => opt.MapFrom(src => src.Price)
            )
            .ForMember(
                dest => dest.Description,
                opt => opt.MapFrom(src => src.Description)
            )
            .ForMember(
                dest => dest.Inventory,
                opt => opt.MapFrom(src => src.Inventory)
            )
            .ForMember(
                dest => dest.Weight,
                opt => opt.MapFrom(src => src.Weight)
            )
            .ForPath(
                dest => dest.CategoryId,
                opt => opt.MapFrom(src => src.CategoryId)
            )
            .ForMember(
                dest => dest.Images,
                opt => opt.MapFrom(src => src.Images.Select(image => new ProductImage
                {
                    ProductImageUrl = image.ImageUrl
                }))
            )
            .ForMember(
                dest => dest.CreatedAt,
                opt => opt.MapFrom(src => DateTime.Now)
            )
            .ForMember(
                dest => dest.UpdatedAt,
                opt => opt.MapFrom(src => DateTime.Now)
            ).ReverseMap();
    }
}

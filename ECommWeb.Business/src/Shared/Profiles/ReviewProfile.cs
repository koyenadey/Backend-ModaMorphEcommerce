using AutoMapper;
using ECommWeb.Business.src.DTO;
using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.Shared.Profiles;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<CreateReviewDTO, Review>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => Guid.NewGuid())
            )
            .ForMember(
                dest => dest.Rating,
                opt => opt.MapFrom(src => src.Rating)
            )
            .ForMember(
                dest => dest.Comment,
                opt => opt.MapFrom(src => src.Comment)
            )
            .ForMember(
                dest => dest.ReviewDate,
                opt => opt.MapFrom(src => DateTime.Now)
            )
            .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId)
            )
            .ForMember(
                dest => dest.Images,
                opt => opt.MapFrom(src => ReviewImageMapper.AddImagesFromSource(src.Images))
            );

        CreateMap<Review, ReadReviewDTO>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            )
            .ForMember(
                dest => dest.Rating,
                opt => opt.MapFrom(src => src.Rating)
            )
            .ForMember(
                dest => dest.User,
                opt => opt.MapFrom(src => src.User)
            )
            .ForMember(
                dest => dest.ReviewDate,
                opt => opt.MapFrom(src => src.ReviewDate)
            )
            .ForMember(
                dest => dest.OrderedProduct,
                opt => opt.MapFrom(src => src.OrderedProduct)
            )
            .ForMember(
                dest => dest.Images,
                opt => opt.MapFrom(
                    src => src.Images.Where(i => i.ReviewId == src.Id)
                    .Select(i => new ReadReviewImageDTO
                    {
                        ImageUrl = i.Image
                    }))
            ).ReverseMap();

        CreateMap<ReviewImage, ReadReviewImageDTO>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            )
            .ForMember(
                dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.Image)
            ).ReverseMap();

        CreateMap<OrderProduct, OrderProductReadDTO>();
        CreateMap<Order, ReadOrderDTO>();
        CreateMap<Product, ProductReadDTO>();
        CreateMap<ProductImage, ProductImageReadDTO>();
        CreateMap<User, UserReadDto>();
    }
}

public static class ReviewImageMapper
{
    public static IEnumerable<ReviewImage> AddImagesFromSource(List<string> images)
    {
        if (images is not null && images.Any())
        {
            return images.Select(image => new ReviewImage
            {
                Image = image
            });
        }
        return null;
    }
}

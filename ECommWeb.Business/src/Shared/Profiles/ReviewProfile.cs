using System.Runtime.InteropServices;
using AutoMapper;
using ECommWeb.Business.src.DTO;
using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.Shared.Profiles;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        //Profile for ReadReviewDto
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
                dest => dest.UserName,
                opt => opt.MapFrom(src => src.User.UserName)
            )
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.OrderedProduct.Product.Name)
            )
            .ForMember(
                dest => dest.ReviewImages,
                opt => opt.MapFrom(src => src.Images)
            );

        CreateMap<ReviewImage, ReviewImageReadDTO>()
            .ForMember(
                dest => dest.ReviewId,
                opt => opt.MapFrom(src => src.ReviewId)
            )
            .ForMember(
                dest => dest.Image,
                opt => opt.MapFrom(src => src.Image)
            );

        //Profile for CreateReviewDto
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
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId)
            )
            .ForMember(
                dest => dest.ReviewDate,
                opt => opt.MapFrom(src => DateTime.Now)
            )
            .ForMember(
                dest => dest.OrderedProductId,
                opt => opt.MapFrom(src => src.OrderedProductId)
            )
            .ForMember(
                dest => dest.Images,
                opt =>
                {
                    opt.PreCondition(src => src.ReviewImages != null);
                    opt.MapFrom(src => src.ReviewImages.Select(i => new ReviewImage
                    {
                        Image = i.Image,
                    }));
                }
            );
    }
}

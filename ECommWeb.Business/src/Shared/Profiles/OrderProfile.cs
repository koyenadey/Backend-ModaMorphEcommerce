using AutoMapper;
using ECommWeb.Business.src.DTO;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.ValueObject;

namespace ECommWeb.Business.src.Shared.Profiles;

public class OrderProfile : Profile
{

    public OrderProfile()
    {

        //Profile for ReadOrderDto
        CreateMap<Order, ReadOrderDTO>()
            .ForMember(
                dest => dest.OrderId,
                opt => opt.MapFrom(src => src.Id)
            )
            .ForMember(
                dest => dest.OrderDate,
                opt => opt.MapFrom(src => src.OrderDate)
            )
            .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(src => src.Status)
            )
            .ForMember(
                dest => dest.UserName,
                opt => opt.MapFrom(src => src.User.UserName)
            )
            .ForMember(
                dest => dest.DateOfDelivery,
                opt => opt.MapFrom(src => src.DateOfDelivery)
            )
            .ForMember(
                dest => dest.Address,
                opt => opt.MapFrom(src => src.Address)
            )
            .ForMember(
                dest => dest.OrderedProducts,
                opt => opt.MapFrom(src => src.OrderedProducts)
            );

        CreateMap<Address, AddressReadDto>();

        CreateMap<OrderProduct, OrderProductReadDTO>()
            .ForMember(
                dest => dest.ProductId,
                opt => opt.MapFrom(src => src.ProductId)
            )
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product.Name)
            )
            .ForMember(
                dest => dest.Quantity,
                opt => opt.MapFrom(src => src.Quantity)
            )
            .ForMember(
                dest => dest.PriceAtPurchase,
                opt => opt.MapFrom(src => src.PriceAtPurchase)
            );

        //Profile for CreateOrderDto
        CreateMap<CreateOrderDTO, Order>()
            .ForMember(
                dest => dest.OrderDate,
                opt => opt.MapFrom(src => DateTime.Now)
            )
            .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(src => Status.processing)
            )
            .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId)
            )
            .ForMember(
                dest => dest.AddressId,
                opt => opt.MapFrom(src => src.AddressId)
            )
            .ForMember(
                dest => dest.OrderedProducts,
                opt => opt.MapFrom(src => src.OrderedProducts)
            );

        CreateMap<OrderProductCreateDTO, OrderProduct>()
            .ForMember(
                dest => dest.OrderId,
                opt => opt.MapFrom(src => Guid.NewGuid())
            )
            .ForMember(
                dest => dest.ProductId,
                opt => opt.MapFrom(src => src.ProductId)
            )
            .ForMember(
                dest => dest.Quantity,
                opt => opt.MapFrom(src => src.Quantity)
            )
            .ForMember(
                dest => dest.PriceAtPurchase,
                opt => opt.MapFrom(src => src.PriceAtPurchase)
            );
    }
}

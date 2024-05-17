using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Business.src.DTO;
using Xunit;

namespace ECommWeb.Test.src.Service.Data;

public class OrderServiceTestData : TheoryData<CreateOrderDTO>
{
    Guid userId = Guid.Parse("b6a509af-a85a-4958-8589-7b4f0119ede8");
    Guid addressId = Guid.Parse("51614cd7-b453-4398-ae98-9d5419d66307");

    List<OrderProductCreateDTO> productsList =
    [
        new OrderProductCreateDTO {
            ProductId = Guid.NewGuid(),
            Quantity = 2,
            PriceAtPurchase = 10.0
        },
         new OrderProductCreateDTO {
            ProductId = Guid.NewGuid(),
            Quantity = 4,
            PriceAtPurchase = 55.0
        },
    ];

    public OrderServiceTestData()
    {
        var createOrderDTO = new CreateOrderDTO
        {
            UserId = userId,
            AddressId = addressId,
            OrderedProducts = productsList
        };

        Add(createOrderDTO);
    }
}

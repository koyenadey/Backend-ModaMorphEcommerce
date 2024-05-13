using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.ValueObject;

namespace Server.Service.src.DTO;

public class ReadOrderDTO
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public Status Status { get; set; }
    public Guid UserId { get; set; }
    public DateTime? DateOfDelivery { get; set; }

    public ReadOrderDTO ReadOrder(Order order)
    {
        return new ReadOrderDTO
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            Status = order.Status,
            UserId = order.UserId,
            DateOfDelivery = order.DateOfDelivery
        };
    }
}
public class CreateOrderDTO
{
    public Guid UserId { get; set; }
    public Guid AddressId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public List<ProductsList> ProductList { get; set; }
    public CreateOrderDTO(Guid userId, Guid addressId, List<ProductsList> productsList)
    {
        UserId = userId;
        AddressId = addressId;
        ProductList = productsList;
    }

    public Order CreateOrder()
    {
        return new Order(UserId, AddressId);
    }
}
public class UpdateOrderDTO
{
    public Status Status { get; set; }
    public DateTime DateOfDelivery { get; set; }

    public UpdateOrderDTO(Status status, DateTime dateOfDelivery, Guid addressId)
    {
        Status = status;
        DateOfDelivery = dateOfDelivery;
    }

    public Order UpdateOrder(Order oldOrder)
    {
        if (oldOrder == null) throw new ArgumentNullException("Order cannot be null");

        oldOrder.Status = Status;
        oldOrder.DateOfDelivery = DateOfDelivery;

        return oldOrder;
    }

}

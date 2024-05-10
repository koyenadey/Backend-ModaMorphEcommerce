namespace ECommWeb.Core.src.Entity;

public class OrderProduct : BaseEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    Order Order { get; set; }
    public Guid ProductId { get; set; }
    Product Product { get; set; }
    public int Quantity { get; set; }

    public OrderProduct(Guid orderId, Guid productId, int quantity)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
}

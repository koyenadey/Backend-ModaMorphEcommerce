namespace ECommWeb.Core.src.Common;

public class ProductsList
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }

    public ProductsList(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}

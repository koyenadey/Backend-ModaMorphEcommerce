using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.DTO;

public class OrderProductReadDTO
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public double PriceAtPurchase { get; set; }
}

public class OrderProductCreateDTO
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public double PriceAtPurchase { get; set; }
}


using ECommWeb.Core.src.ValueObject;

namespace ECommWeb.Core.src.Entity;

public class Order : BaseEntity
{
    public DateTime OrderDate { get; set; }
    public Status Status { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime? DateOfDelivery { get; set; }
    public Guid AddressId { get; set; }
    public Address Address { get; set; }
    public IEnumerable<OrderProduct> OrderedProducts { get; set; } // A order has many products ordered..relationship eshtablished
}

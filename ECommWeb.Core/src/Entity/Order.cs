using ECommWeb.Core.src.ValueObject;

namespace ECommWeb.Core.src.Entity;

public class Order : BaseEntity
{

    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public Status Status { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime? DateOfDelivery { get; set; }
    public Guid AddressId { get; set; }
    public Address Address { get; set; }

    public Order(Guid userId, Guid addressId)
    {
        Id = Guid.NewGuid();
        OrderDate = DateTime.Now;
        Status = Status.processing;
        UserId = userId;
        AddressId = addressId;
    }
}

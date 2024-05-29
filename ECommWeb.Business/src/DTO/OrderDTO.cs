using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.ValueObject;

namespace ECommWeb.Business.src.DTO;

public class ReadOrderDTO
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public Status Status { get; set; }
    public UserReadDto User { get; set; }
    public DateTime? DateOfDelivery { get; set; }
    public AddressReadDto Address { get; set; }
    public IEnumerable<OrderProductReadDTO> OrderedProducts { get; set; }
}
public class CreateOrderDTO
{
    public Guid UserId { get; set; }
    public Guid AddressId { get; set; }
    public IEnumerable<OrderProductCreateDTO> OrderedProducts { get; set; }

}
public class UpdateOrderDTO
{
    public Status Status { get; set; }
    public string DateOfDelivery { get; set; }

}

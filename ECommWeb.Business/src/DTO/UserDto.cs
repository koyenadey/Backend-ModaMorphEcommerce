using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.ValueObject;

namespace ECommWeb.Business.src.DTO;

public class UserReadDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    public string? Avatar { get; set; }

}

public class UserUpdateDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public byte[] Salt { get; set; }
}

public class UserCreateDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Avatar { get; set; }
    public string AddresLine1 { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Postcode { get; set; }
    public string PhoneNumber { get; set; }
    public string Landmark { get; set; }
}

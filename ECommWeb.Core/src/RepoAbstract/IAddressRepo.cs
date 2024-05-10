using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.Common;

namespace ECommWeb.Core.src.RepoAbstract;

public interface IAddressRepo
{
    Task<Address> GetAddressByIdAsync(Guid id);
    Task<IEnumerable<Address>> GetAddressesByUserAsync(Guid userId, QueryOptions? options);
    // Task<IEnumerable<Address>> GetAddressesByParamsAsync(QueryOptions options);
    Task<Address> UpdateAddressByIdAsync(Address address);
    Task<bool> DeleteAddressByIdAsync(Guid id);
    Task<Address> CreateAddressAsync(Address address);
    Task<bool> SetDefaultAddressAsync(Guid userId, Guid addressId);
    Task<Address> GetDefaultAddressAsync(Guid userId);
}

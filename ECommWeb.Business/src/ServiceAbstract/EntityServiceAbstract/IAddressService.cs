using ECommWeb.Business.src.DTO;
using ECommWeb.Core.src.Common;

namespace ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;

public interface IAddressService
{
    Task<AddressReadDto> GetAddressByIdAsync(Guid id);
    Task<IEnumerable<AddressReadDto>> GetAddressesByUserAsync(Guid userId, QueryOptions? options);

    Task<AddressReadDto> UpdateAddressByIdAsync(AddressUpdateDto address);
    Task<bool> DeleteAddressByIdAsync(Guid id);
    Task<AddressReadDto> CreateAddressAsync(Guid userId, AddressCreateDto address);
    Task<bool> SetDefaultAddressAsync(Guid userId, Guid addressId);
    Task<AddressReadDto> GetDefaultAddressAsync(Guid userId);
}

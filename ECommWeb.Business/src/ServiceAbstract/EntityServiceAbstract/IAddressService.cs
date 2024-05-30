using ECommWeb.Business.src.DTO;
using ECommWeb.Core.src.Common;

namespace ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;

public interface IAddressService
{
    Task<AddressReadDto> GetAddressByIdAsync(Guid id);
    Task<IEnumerable<AddressReadDto>> GetAddressesByUserAsync(Guid userId, QueryOptions? options);
    Task<AddressReadDto> UpdateAddressByIdAsync(Guid addressId, AddressUpdateDto address);
    Task<Guid> DeleteAddressByIdAsync(Guid id);
    Task<AddressReadDto> CreateAddressAsync(AddressCreateDto address);
    Task<AddressReadDto> SetDefaultAddressAsync(Guid userId, Guid addressId);
    Task<AddressReadDto> GetDefaultAddressAsync(Guid userId);
}

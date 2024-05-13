using ECommWeb.Core.src.Common;
using ECommWeb.Business.src.DTO;

namespace ECommWeb.Business.src.ServiceAbstract.AuthServiceAbstract;

// public interface IAuthService
// {
//     Task<string> LoginAsync(UserCredential userCredential);
// }
public interface IAuthService
{
    public Task<string> Login(UserCredential credential);
    public Task<UserReadDto> GetCurrentProfile(Guid id);
}
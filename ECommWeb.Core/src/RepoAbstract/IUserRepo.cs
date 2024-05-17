using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;

namespace ECommWeb.Core.src.RepoAbstract;

public interface IUserRepo
{
    Task<User> GetUserByIdAsync(Guid id);
    Task<IEnumerable<User>> GetAllUsersAsync(QueryOptions options);
    Task<User> UpdateUserByIdAsync(User user);
    Task<bool> DeleteUserByIdAsync(Guid id);
    Task<User> CreateUserAsync(User user, Address address);
    Task<bool> ChangePasswordAsync(Guid userId, string newPassword, byte[] salt);
    Task<bool> CheckEmailAsync(string email);
    Task<User> GetUserByCredentialsAsync(UserCredential userCredential);

}

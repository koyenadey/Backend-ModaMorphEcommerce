using Microsoft.EntityFrameworkCore;
using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Core.src.ValueObject;
using ECommWeb.Infrastructure.src.Database;

namespace ECommWeb.Infrastructure.src.Repo;

public class UserRepo : IUserRepo
{
    private readonly AppDbContext _context;
    public UserRepo(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> ChangePasswordAsync(Guid userId, string newPassword, byte[] salt)
    {
        var user = await GetUserByIdAsync(userId);

        if (user == null)
        {
            return false;
        }
        // Update the user's password
        user.Password = newPassword;
        user.Salt = salt;
        // Save the changes to the database
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CheckEmailAsync(string email)
    {
        var result = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (result == null) return false;
        return true;
    }

    public async Task<User> CreateUserAsync(User user, Address address)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.Addresses.AddAsync(address);
                await _context.SaveChangesAsync();


                //Fetch the users again by loading up the addresses
                var addedUser = await _context.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Id == user.Id);

                //Now commit the transaction
                await transaction.CommitAsync();

                //return the completely loaded uswe
                return addedUser;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }

    public async Task<bool> DeleteUserByIdAsync(Guid id)
    {
        var userToDelete = await GetUserByIdAsync(id);

        // If user with the specified ID exists, delete it
        if (userToDelete != null)
        {
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
            return true; // Return true indicating successful deletion
        }
        else
        {
            return false; // Return false indicating user not found or deletion failed
        }
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(QueryOptions options)
    {
        IQueryable<User> query = _context.Users;

        // Apply search filter if a search key is provided
        if (!string.IsNullOrWhiteSpace(options.SearchKey))
        {
            query = query.Where(u => u.UserName.Contains(options.SearchKey) ||
                                      u.Email.Contains(options.SearchKey));
        }

        // Apply sorting if sort type and sort order are specified
        if (options.sortType.HasValue && options.sortOrder.HasValue)
        {
            switch (options.sortType.Value)
            {
                case SortType.byName:
                    query = options.sortOrder.Value == SortOrder.asc ? query.OrderBy(u => u.UserName) : query.OrderByDescending(u => u.UserName);
                    break;
            }
        }

        // Apply pagination
        var pgNo = options.PageNo;
        var pgSize = options.PageSize;
        int skipCount = options.PageSize * options.PageNo;
        query = query.Skip((pgNo - 1) * pgSize).Take(pgSize);

        return await query.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task<User> UpdateUserByIdAsync(User UserUpdateInfo)
    {
        var userToUpdate = await GetUserByIdAsync(UserUpdateInfo.Id);
        if (userToUpdate == null)
        {
            return null;
        }

        userToUpdate.UserName = UserUpdateInfo.UserName;
        userToUpdate.Avatar = UserUpdateInfo.Avatar;
        // Or -- ???
        _context.Users.Update(UserUpdateInfo);

        // Save changes to the database
        await _context.SaveChangesAsync();

        return userToUpdate;

    }
    public async Task<User> GetUserByCredentialsAsync(UserCredential userCredential)
    {
        var foundUser = await _context.Users.FirstOrDefaultAsync(user => user.Email == userCredential.Email);
        return foundUser;
    }
}



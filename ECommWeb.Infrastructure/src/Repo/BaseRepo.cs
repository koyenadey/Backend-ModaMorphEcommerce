using Microsoft.EntityFrameworkCore;
using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Infrastructure.src.Database;

namespace ECommWeb.Infrastructure.src.Repo;

public class BaseRepo<T> : IBaseRepo<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _data;

    public BaseRepo(AppDbContext context)
    {
        _context = context;
        _data = _context.Set<T>();
    }

    public virtual async Task<T> CreateOneAsync(T createObject)
    {
        await _data.AddAsync(createObject);
        await _context.SaveChangesAsync();
        return createObject;
    }

    public virtual Task<T> DeleteOneByIdAsync(T deleteObject)
    {
        _data.Remove(deleteObject);
        _context.SaveChangesAsync();
        return Task.FromResult(deleteObject);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(QueryOptions options)
    {
        return await _data.ToListAsync();
    }

    public virtual async Task<T> GetOneByIdAsync(Guid id)
    {
        var data = await _data.FirstOrDefaultAsync(c => c.Id == id);
        return data;
    }

    public virtual async Task<T> UpdateOneByIdAsync(T updateObject)
    {
        _data.Update(updateObject);
        await _context.SaveChangesAsync();
        return updateObject;
    }
}
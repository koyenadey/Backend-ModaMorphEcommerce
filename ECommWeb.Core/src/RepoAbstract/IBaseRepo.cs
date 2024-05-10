using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.Common;

namespace ECommWeb.Core.src.RepoAbstract;

public interface IBaseRepo<T> where T : BaseEntity
{
    public Task<IEnumerable<T>> GetAllAsync(QueryOptions options);
    public Task<T> GetOneByIdAsync(Guid id);
    public Task<T> CreateOneAsync(T createObject);
    public Task<T> UpdateOneByIdAsync(T updateObject);
    public Task<bool> DeleteOneByIdAsync(T deleteObject);
}
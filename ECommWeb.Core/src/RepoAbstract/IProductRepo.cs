using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;

namespace ECommWeb.Core.src.RepoAbstract;

public interface IProductRepo : IBaseRepo<Product>
{
    public Task<int> GetProductsCount(string SearchKey);
    public Task<int> GetProductsCountByCategory(Guid categoryId, string SearchKey);
    IEnumerable<Product> GetByCategory(Guid categoryId, QueryOptions options);
    public Task<IEnumerable<Product>> GetMostPurchased(int topNumber);
}
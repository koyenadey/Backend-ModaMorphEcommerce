using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;

namespace ECommWeb.Core.src.RepoAbstract;

public interface IProductRepo : IBaseRepo<Product>
{
    Task<int> GetProductsCount(string SearchKey);
    Task<int> GetProductsCountByCategory(Guid categoryId, string SearchKey);
    IEnumerable<Product> GetByCategory(Guid categoryId, QueryOptions options);
    IEnumerable<Product> GetMostPurchased(int topNumber);
}
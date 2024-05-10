using ECommWeb.Core.src.Entity;

namespace ECommWeb.Core.src.RepoAbstract;

public interface IProductRepo : IBaseRepo<Product>
{
    IEnumerable<Product> GetByCategory(Guid categoryId);
    IEnumerable<Product> GetMostPurchased(int topNumber);
}
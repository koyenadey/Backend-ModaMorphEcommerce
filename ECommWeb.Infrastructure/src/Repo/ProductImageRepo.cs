using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Infrastructure.src.Database;

namespace ECommWeb.Infrastructure.src.Repo;
public class ProductImageRepo : BaseRepo<ProductImage>, IProductImageRepo
{
    public ProductImageRepo(AppDbContext context) : base(context)
    {
    }
}
using Microsoft.EntityFrameworkCore;
using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Core.src.ValueObject;
using ECommWeb.Infrastructure.src.Database;
using ECommWeb.Infrastructure.src.Repo;

namespace ECommWeb.Infrastructure.src.Repo;

public class ProductRepo : BaseRepo<Product>, IProductRepo
{
    //private DbSet<OrderProduct> _orderProducts;
    public ProductRepo(AppDbContext context) : base(context)
    {
        //_orderProducts = context.OrderedProducts;
    }

    public async Task<int> GetProductsCount(string SearchKey)
    {
        if (string.IsNullOrWhiteSpace(SearchKey))
        {
            return await _context.Products.CountAsync();
        }
        return await _context.Products
            .Where(p => p.Name.ToLower().Contains(SearchKey.ToLower())
                || p.Description.ToLower().Contains(SearchKey.ToLower()))
            .CountAsync();
    }

    public async Task<int> GetProductsCountByCategory(Guid categoryId, string SearchKey)
    {
        if (string.IsNullOrWhiteSpace(SearchKey))
        {
            return await _context.Products.Where(p => p.CategoryId == categoryId).CountAsync();
        }
        return await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .Where(p => p.Name.ToLower().Contains(SearchKey.ToLower())
                || p.Description.ToLower().Contains(SearchKey.ToLower()))
            .CountAsync();

    }

    public override async Task<IEnumerable<Product>> GetAllAsync(QueryOptions options)
    {
        var pgNo = options.PageNo;
        var pgSize = options.PageSize;
        var allData = _context.Products.AsQueryable().Include("Images").Include("Category");

        if (!string.IsNullOrWhiteSpace(options.SearchKey))
        {
            var searchKeyLower = options.SearchKey.ToLower();

            allData = allData.Where(p => p.Name.ToLower().Contains(searchKeyLower)
                                      || p.Description.ToLower().Contains(searchKeyLower
                                    ));

        }
        allData = allData.Skip((pgNo - 1) * pgSize).Take(pgSize);
        if (options.sortType == SortType.byTitle && options.sortOrder == SortOrder.asc)
        {
            return allData.OrderBy(item => item.Name);
        }
        if (options.sortType == SortType.byTitle && options.sortOrder == SortOrder.desc)
        {
            return allData.OrderByDescending(item => item.Name);
        }
        if (options.sortType == SortType.byPrice && options.sortOrder == SortOrder.asc)
        {
            return allData.OrderBy(item => item.Price);
        }

        return allData.OrderByDescending(item => item.Price).ToArray();
    }

    public override async Task<Product> GetOneByIdAsync(Guid id)
    {
        var productFound = _data.Include("Images").Include("Category");
        return productFound.First(product => product.Id == id);
    }

    public IEnumerable<Product> GetByCategory(Guid categoryId, QueryOptions options)
    {
        var pgNo = options.PageNo;
        var pgSize = options.PageSize;
        var allData = _context.Products.AsQueryable().Include("Images").Include("Category")
                            .Where(product => product.CategoryId == categoryId);

        if (!string.IsNullOrWhiteSpace(options.SearchKey))
        {
            var searchKeyLower = options.SearchKey.ToLower();

            allData = allData.Where(p => p.Name.ToLower().Contains(searchKeyLower)
                                      || p.Description.ToLower().Contains(searchKeyLower
                                    ));

        }
        allData = allData.Skip((pgNo - 1) * pgSize).Take(pgSize);
        if (options.sortType == SortType.byTitle && options.sortOrder == SortOrder.asc)
        {
            return allData.OrderBy(item => item.Name);
        }
        if (options.sortType == SortType.byTitle && options.sortOrder == SortOrder.desc)
        {
            return allData.OrderByDescending(item => item.Name);
        }
        if (options.sortType == SortType.byPrice && options.sortOrder == SortOrder.asc)
        {
            return allData.OrderBy(item => item.Price);
        }
        return allData.OrderByDescending(item => item.Price).ToArray();
    }

    public override async Task<Product> CreateOneAsync(Product product)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                await _data.AddAsync(product);
                await _context.ProductImages.AddRangeAsync(product.Images);

                //Save the changes
                await _context.SaveChangesAsync();

                //Fetch the product again by loading up the product Images
                var productWithImages = await _data.Include("Images").Include("Category").FirstAsync(p => p.Id == product.Id);

                //Now commit the transaction
                await transaction.CommitAsync();

                //return the completely loaded product
                return productWithImages;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }


    public override async Task<Product> UpdateOneByIdAsync(Product product)
    {
        try
        {
            //First get the product from the database
            var productToUpdate = await GetOneByIdAsync(product.Id);

            //Now update the product
            productToUpdate.Inventory = product.Inventory;
            //Save the changes
            await _context.SaveChangesAsync();

            var productUpdated = await _context.Products.Include(p => p.Images).FirstAsync(p => p.Id == productToUpdate.Id);

            //return the completely loaded product
            return productUpdated;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<IEnumerable<Product>> GetMostPurchased(int topNumber)
    {

        var mostPurchasedProducts = await _context.OrderedProducts
                                    .GroupBy(op => op.ProductId)
                                    .OrderByDescending(g => g.Count())
                                    .Select(g => g.Key)
                                    .Take(topNumber)
                                    .ToListAsync();


        var products = await _context.Products
                                .Where(p => mostPurchasedProducts.Contains(p.Id))
                                .Include(p => p.Images)
                                .Include(p => p.Category)
                                .ToListAsync();

        return products;
    }

    public override async Task<Product> DeleteOneByIdAsync(Product product)
    {
        var productFound = await _data.FirstOrDefaultAsync(p => p.Id == product.Id);

        _data.Remove(productFound);
        await _context.SaveChangesAsync();
        return product;

    }

}
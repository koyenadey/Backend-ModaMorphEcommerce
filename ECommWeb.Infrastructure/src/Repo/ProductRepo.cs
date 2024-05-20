using Microsoft.EntityFrameworkCore;
using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Core.src.ValueObject;
using ECommWeb.Infrastructure.src.Database;
using ECommWeb.Infrastructure.src.Repo;
using Microsoft.AspNetCore.Mvc;

namespace ECommWeb.Infrastructure.src.Repo;

public class ProductRepo : BaseRepo<Product>, IProductRepo
{
    //private DbSet<OrderProduct> _orderProducts;
    public ProductRepo(AppDbContext context) : base(context)
    {
        //_orderProducts = context.OrderedProducts;
    }

    public async Task<int> GetProductsCount()
    {
        return await _context.Products.CountAsync();
    }


    public override async Task<IEnumerable<Product>> GetAllAsync(QueryOptions options)
    {
        var allData = _data.Include("Images").Include("Category").Skip(options.PageNo).Take(options.PageSize);

        if (options.sortType == SortType.byTitle && options.sortOrder == SortOrder.asc)
        {
            return allData.OrderBy(item => item.Name).ToArray();
        }
        if (options.sortType == SortType.byTitle && options.sortOrder == SortOrder.desc)
        {
            return allData.OrderByDescending(item => item.Name).ToArray();
        }
        if (options.sortType == SortType.byPrice && options.sortOrder == SortOrder.asc)
        {
            return allData.OrderBy(item => item.Price).ToArray();
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
        var pageNo = options.PageNo;
        var pageSize = options.PageSize;

        return _data.Include("Images").Include("Category")
                    .Where(product => product.CategoryId == categoryId)
                    .Skip((1 - pageNo) * pageSize)
                    .Take(pageSize);
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
    public IEnumerable<Product> GetMostPurchased(int topNumber)
    {
        // var mostPurchasedProducts = _orderProducts
        //         .GroupBy(orderProduct => orderProduct.Product.Id)
        //         .Select(group => new
        //         {
        //             ProductId = group.Key,
        //             TotalQuantity = group.Sum(item => item.Quantity)
        //         })
        //         .OrderByDescending(item => item.TotalQuantity)
        //         .Take(topNumber)
        //         .Join(_data.Include("Images").Include("Category"),
        //             orderItem => orderItem.ProductId,
        //             product => product.Id,
        //             (orderItem, product) => product)
        //         .ToArray();

        // return mostPurchasedProducts;
        return null;
    }

    public override async Task<bool> DeleteOneByIdAsync(Product product)
    {
        var productFound = await _data.FirstOrDefaultAsync(p => p.Id == product.Id);
        if (productFound == null) return false;
        else
        {
            _data.Remove(productFound);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
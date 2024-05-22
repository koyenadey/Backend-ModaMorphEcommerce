using Microsoft.EntityFrameworkCore;
using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Infrastructure.src.Database;

namespace ECommWeb.Infrastructure.src.Repo
{
    public class CategoryRepo : BaseRepo<Category>, ICategoryRepo
    {
        public CategoryRepo(AppDbContext context) : base(context)
        {
        }

        public virtual async Task<IEnumerable<Category>> GetAllAsync(QueryOptions options)
        {
            return await _context.Categories.ToListAsync();
        }

        public virtual async Task<Category> GetOneByIdAsync(Guid id)
        {
            return await _data.FirstOrDefaultAsync(c => c.Id == id);
        }


        public virtual async Task<Category> CreateOneAsync(Category category)
        {
            _data.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public virtual async Task<Category> UpdateOneByIdAsync(Category category)
        {
            try
            {
                //First get the product from the database
                var categoryToUpdate = await GetOneByIdAsync(category.Id);

                //Now update the product
                categoryToUpdate.Name = category.Name ?? categoryToUpdate.Name;
                categoryToUpdate.Image = category.Image ?? categoryToUpdate.Image;

                //Save the changes
                await _context.SaveChangesAsync();

                //return the completely loaded product
                return categoryToUpdate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override async Task<Category> DeleteOneByIdAsync(Category category)
        {
            var categoryFound = await _data.FirstOrDefaultAsync(c => c.Id == category.Id);

            _data.Remove(categoryFound);
            await _context.SaveChangesAsync();
            return category;

        }
    }
}
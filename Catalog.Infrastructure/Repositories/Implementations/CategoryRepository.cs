using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Domain.Enteties;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories.Implementations
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(CatalogDbContext context) : base(context) { }

        public async Task<Category?> GetCategoryWithProductsAsync(Guid id)
        {
            return await _dbSet
                .Include(c => c.ProductCategories)
                    .ThenInclude(pc => pc.Product)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }
    }
}

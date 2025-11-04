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
    public class ProductCategoryRepository : GenericRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(CatalogDbContext context) : base(context) { }

        public async Task<bool> ExistsAsync(Guid productId, Guid categoryId)
        {
            return await _dbSet.AnyAsync(pc => pc.ProductId == productId && pc.CategoryId == categoryId);
        }
    }
}

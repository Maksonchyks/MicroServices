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
    public class ProductImageRepository : GenericRepository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(CatalogDbContext context) : base(context) { }

        public async Task<IReadOnlyList<ProductImage>> GetAllByProductIdAsync(Guid productId)
        {
            return await _dbSet.Where(img => img.ProductId == productId).ToListAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Domain.Enteties;

namespace Catalog.Infrastructure.Repositories.Interfaces
{
    public interface IProductDetailRepository : IGenericRepository<ProductDetail>
    {
        Task<ProductDetail?> GetByProductIdAsync(Guid productId);
    }
}

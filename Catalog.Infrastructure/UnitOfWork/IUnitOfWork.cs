using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Infrastructure.Repositories.Interfaces;

namespace Catalog.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IProductImageRepository ProductImages { get; }
        IProductDetailRepository ProductDetails { get; }
        IProductCategoryRepository ProductCategories { get; }

        Task<int> SaveChangesAsync();
    }
}

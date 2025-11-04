using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Repositories.Implementations;
using Catalog.Infrastructure.Repositories.Interfaces;

namespace Catalog.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CatalogDbContext _context;

        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get; }
        public IProductDetailRepository ProductDetails { get; }
        public IProductImageRepository ProductImages { get; }
        public IProductCategoryRepository ProductCategories { get; }

        public UnitOfWork(CatalogDbContext context)
        {
            _context = context;
            Products = new ProductRepository(_context);
            Categories = new CategoryRepository(_context);
            ProductDetails = new ProductDetailRepository(_context);
            ProductImages = new ProductImageRepository(_context);
            ProductCategories = new ProductCategoryRepository(_context);
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Domain.Enteties;

namespace Catalog.Infrastructure.Specification.Specifications
{
    public class ProductsByCategoryIdSpec : BaseSpecification<Product>
    {
        public ProductsByCategoryIdSpec(Guid categoryId, int skip, int take)
            : base(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId))
        {
            AddInclude(p => p.ProductImages);
            AddInclude(p => p.ProductDetail);

            ApplyOrderBy(p => p.Name);

            ApplyPaging(skip, take);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Domain.Enteties;
using Catalog.Infrastructure.Specification;

namespace Catalog.Application.Specifications
{
    public class ProductsByCategoryIdSpec : BaseSpecification<Product>
    {
        public ProductsByCategoryIdSpec(Guid categoryId, int skip, int take)
            : base(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId))
        {
            AddInclude(p => p.ProductImages);

            ApplyOrderBy(p => p.Name);
            ApplyPaging(skip, take);
        }
    }
}

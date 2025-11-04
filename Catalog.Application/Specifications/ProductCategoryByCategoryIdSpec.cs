using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Domain.Enteties;
using Catalog.Infrastructure.Specification;

namespace Catalog.Application.Specifications
{
    public class ProductCategoryByCategoryIdSpec : BaseSpecification<ProductCategory>
    {
        public ProductCategoryByCategoryIdSpec(Guid categoryId)
            : base(pc => pc.CategoryId == categoryId)
        {
        }
    }
}

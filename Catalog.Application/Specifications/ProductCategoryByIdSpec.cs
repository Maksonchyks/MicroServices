using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Domain.Enteties;
using Catalog.Infrastructure.Specification;

namespace Catalog.Application.Specifications
{
    public class ProductCategoryByIdSpec : BaseSpecification<ProductCategory>
    {
        public ProductCategoryByIdSpec(Guid productId, Guid categoryId)
            : base(pc => pc.ProductId == productId && pc.CategoryId == categoryId)
        {
        }
    }

}

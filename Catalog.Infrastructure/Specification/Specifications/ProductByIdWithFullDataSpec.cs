using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Domain.Enteties;

namespace Catalog.Infrastructure.Specification.Specifications
{
    public class ProductByIdWithFullDataSpec : BaseSpecification<Product>
    {
        public ProductByIdWithFullDataSpec(Guid productId)
            : base(p => p.Id == productId)
        {
            AddInclude(p => p.ProductDetail);
            AddInclude(p => p.ProductImages);
            AddInclude(p => p.ProductCategories);
        }
    }
}

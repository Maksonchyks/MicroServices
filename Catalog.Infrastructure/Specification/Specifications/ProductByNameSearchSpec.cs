using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Domain.Enteties;

namespace Catalog.Infrastructure.Specification.Specifications
{
    public class ProductByNameSearchSpec : BaseSpecification<Product>
    {
        public ProductByNameSearchSpec(string searchTerm, int skip, int take)
            : base(p => string.IsNullOrWhiteSpace(searchTerm) || p.Name.ToLower().Contains(searchTerm.ToLower()))
        {
            ApplyOrderBy(p => p.Name);

            ApplyPaging(skip, take);
        }
    }
}

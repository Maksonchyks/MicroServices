using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Domain.Enteties;

namespace Catalog.Infrastructure.Specification.Specifications
{
    public class CategoriesWithProductCountSpec : BaseSpecification<Category>
    {
        public CategoriesWithProductCountSpec()
            : base(c => true) 
        {
            AddInclude(c => c.ProductCategories);

            ApplyOrderBy(c => c.Name);
        }
    }
}

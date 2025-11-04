using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Application.Pagination;
using Catalog.Domain.Enteties;
using Catalog.Infrastructure.Specification;

namespace Catalog.Application.Specifications
{
    public class CategoriesWithProductCountSpec : BaseSpecification<Category>
    {
        public CategoriesWithProductCountSpec()
            : base(c => true)
        {
            AddInclude(c => c.ProductCategories);
            ApplyOrderBy(c => c.Name);
        }

        public CategoriesWithProductCountSpec(PaginationParams pagination)
            : base(c => true)
        {
            AddInclude(c => c.ProductCategories);
            ApplyOrderBy(c => c.Name);
            ApplyPaging((pagination.PageNumber - 1) * pagination.PageSize, pagination.PageSize);
        }
    }
}

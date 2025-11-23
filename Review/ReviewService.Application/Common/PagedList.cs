using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewService.Application.Common
{
    public class PagedList<T>
    {
        public List<T> Items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public bool HasNextPage => Page * PageSize < TotalCount;
        public bool HasPreviousPage => Page > 1;

        public PagedList(List<T> items, int totalCount, int page, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }
    }
}

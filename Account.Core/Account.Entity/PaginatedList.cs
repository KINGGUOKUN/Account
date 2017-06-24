using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Entity
{
    public class PaginatedList<T>
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int Count { get; private set; }
        public List<T> Items { get; }

        public PaginatedList(int pageIndex, int pageSize, int count, List<T> items)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.Count = count;
            this.Items = items ?? new List<T>();
        }
    }
}

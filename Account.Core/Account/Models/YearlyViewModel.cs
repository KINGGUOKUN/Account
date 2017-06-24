using Account.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Models
{
    public class YearlyViewModel : PaginatedList<Yearly>
    {
        public YearlyViewModel(int pageIndex, int pageSize, int count, List<Yearly> items)
           : base(pageIndex, pageSize, count, items)
        {
        }

        /// <summary>
        /// 起始时间
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public int End { get; set; }
    }
}

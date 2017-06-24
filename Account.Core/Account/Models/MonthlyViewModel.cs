using Account.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Models
{
    public class MonthlyViewModel : PaginatedList<Monthly>
    {
        public MonthlyViewModel(int pageIndex, int pageSize, int count, List<Monthly> items)
           : base(pageIndex, pageSize, count, items)
        {
        }

        /// <summary>
        /// 起始时间
        /// </summary>
        public string Start { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string End { get; set; }
    }
}

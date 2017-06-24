using Account.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Models
{
    public class DailyViewModel : PaginatedList<Daily>
    {
        public DailyViewModel(int pageIndex, int pageSize, int count, List<Daily> items)
           : base(pageIndex, pageSize, count, items)
        {
        }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime End { get; set; }
    }
}

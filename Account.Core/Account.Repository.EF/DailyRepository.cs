using System;
using System.Collections.Generic;
using System.Text;
using Account.Repository.Contract;
using System.Threading.Tasks;
using Account.Entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Account.Repository.EF
{
    public class DailyRepository : Repository<Daily>, IDailyRepository
    {
        private readonly AccountContext _context;

        public DailyRepository(AccountContext context)
            :base(context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Daily>> GetDailys(DateTime start, DateTime end, int pageIndex, int pageSize)
        {
            var source = _context.Dailys.Where(x => x.Date >= start && x.Date < new DateTime(end.Year, end.Month, end.Day).AddDays(1));
            int count = await source.CountAsync();
            List<Daily> dailys = null;
            if (count > 0)
            {
                dailys = await source.OrderBy(x => x.Date).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            }

            return new PaginatedList<Daily>(pageIndex, pageSize, count, dailys ?? new List<Daily>());
        }
    }
}

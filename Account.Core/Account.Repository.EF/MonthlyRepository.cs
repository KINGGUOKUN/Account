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
    public class MonthlyRepository : IMontylyRepository
    {
        private readonly AccountContext _context;

        public MonthlyRepository(AccountContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Monthly>> GetMonthlys(string start, string end, int pageIndex, int pageSize)
        {
            var source = _context.Dailys
                .Where(x => x.Date >= DateTime.Parse(start) && x.Date <= DateTime.Parse(end).AddMonths(1).AddSeconds(-1))
                .GroupBy(x => x.Date.ToString("yyyy-MM"), (k, v) =>
                new Monthly
                {
                    ID = Guid.NewGuid().ToString(),
                    Month = k,
                    Cost = v.Sum(x => x.Cost)
                });
            int count = await source.CountAsync();
            List<Monthly> months = null;
            if (count > 0)
            {
                months = await source.OrderBy(x => x.Month).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            }

            return new PaginatedList<Monthly>(pageIndex, pageSize, count, months ?? new List<Monthly>());
        }
    }
}

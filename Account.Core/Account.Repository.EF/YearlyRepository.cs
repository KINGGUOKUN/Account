using Account.Entity;
using Account.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Account.Repository.EF
{
    public class YearlyRepository : IYearlyRepository
    {
        private readonly AccountContext _context;

        public YearlyRepository(AccountContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Yearly>> GetYearlys(int start, int end, int pageIndex, int pageSize)
        {
            var source = _context.Dailys
                .Where(x => x.Date.Year >= start && x.Date.Year <= end)
                .GroupBy(x => x.Date.Year, (k, v) =>
                new Yearly
                {
                    ID = Guid.NewGuid().ToString(),
                    Year = k,
                    Cost = v.Sum(x => x.Cost)
                });
            int count = await source.CountAsync();
            List<Yearly> yearlys = null;
            if (count > 0)
            {
                yearlys = await source.OrderBy(x => x.Year).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            }

            return new PaginatedList<Yearly>(pageIndex, pageSize, count, yearlys ?? new List<Yearly>());
        }
    }
}

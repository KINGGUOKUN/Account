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
            var source = dbSet.Where(x => x.Date >= start && x.Date < new DateTime(end.Year, end.Month, end.Day).AddDays(1));
            int count = await source.CountAsync();
            List<Daily> dailys = null;
            if (count > 0)
            {
                dailys = await source.OrderBy(x => x.Date).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            }

            return new PaginatedList<Daily>(pageIndex, pageSize, count, dailys ?? new List<Daily>());
        }

        public async Task<PaginatedList<Monthly>> GetMonthlys(string start, string end, int pageIndex, int pageSize)
        {
            var source = dbSet
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

        public async Task<PaginatedList<Yearly>> GetYearlys(int start, int end, int pageIndex, int pageSize)
        {
            var source = dbSet
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

using System;
using System.Collections.Generic;
using System.Text;
using Account.Repository.Contract;
using Account.Entity;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Account.Common;

namespace Account.Repository.EF
{
    public class ManifestRepository : Repository<Manifest>, IManifestRepository
    {

        public ManifestRepository(AccountContext context)
            :base(context)
        {
        }

        public async Task<PaginatedList<Manifest>> GetManifests(DateTime start, DateTime end, int pageIndex, int pageSize)
        {
            var source = dbSet.Where(x => x.Date >= start && x.Date < new DateTime(end.Year, end.Month, end.Day).AddDays(1));
            int count = await source.CountAsync();
            List<Manifest> manifests = null;
            if (count > 0)
            {
                manifests = await source.OrderBy(x => x.Date).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            }

            return new PaginatedList<Manifest>(pageIndex, pageSize, count, manifests ?? new List<Manifest>());
        }
    }
}

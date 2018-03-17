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
        private readonly AccountContext _context;

        public ManifestRepository(AccountContext context)
            :base(context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Manifest>> GetManifests(DateTime start, DateTime end, int pageIndex, int pageSize)
        {
            var source = _context.Manifests.Where(x => x.Date >= start && x.Date < new DateTime(end.Year, end.Month, end.Day).AddDays(1));
            int count = await source.CountAsync();
            List<Manifest> manifests = null;
            if (count > 0)
            {
                manifests = await source.OrderBy(x => x.Date).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            }

            return new PaginatedList<Manifest>(pageIndex, pageSize, count, manifests ?? new List<Manifest>());
        }

        public Manifest GetManifestById(string ID)
        {
            return _context.Manifests.SingleOrDefault(x => x.ID == ID);
        }

        public Manifest AddManifest(Manifest manifest)
        {
            _context.Add(manifest);

            var daily = _context.Dailys.FirstOrDefault(x => x.Date.Date == manifest.Date.Date);
            if (daily != null)
            {
                daily.Cost += manifest.Cost;
                _context.Update(daily);
            }
            else
            {
                daily = new Daily
                {
                    ID = Guid.NewGuid().ToString(),
                    Date = manifest.Date,
                    Cost = manifest.Cost
                };
                _context.Add(daily);
            }

            _context.SaveChanges();

            return manifest;
        }

        public bool UpdateManifest(Manifest manifest)
        {
            var currentManifest = _context.Manifests.AsNoTracking().SingleOrDefault(x => x.ID == manifest.ID);
            if (currentManifest == null)
            {
                throw new BusinessException((int)ErrorCode.NotFound, "该消费明细已删除");
            }

            _context.Update(manifest);

            if (manifest.Date.Date == currentManifest.Date.Date)
            {
                var daily = _context.Dailys.FirstOrDefault(x => x.Date.Date == manifest.Date.Date);
                daily.Cost += manifest.Cost - currentManifest.Cost;
                _context.Update(daily);
            }
            else
            {
                var currentDaily = _context.Dailys.FirstOrDefault(x => x.Date.Date == currentManifest.Date.Date);
                if (_context.Manifests.Any(x => x.Date.Date == currentManifest.Date.Date && x.ID != manifest.ID))
                {
                    currentDaily.Cost -= manifest.Cost;
                    _context.Update(currentDaily);
                }
                else
                {
                    _context.Remove(currentDaily);
                }

                var newDaily = _context.Dailys.FirstOrDefault(x => x.Date.Date == manifest.Date.Date);
                if (newDaily == null)
                {
                    newDaily = new Daily
                    {
                        ID = Guid.NewGuid().ToString(),
                        Date = manifest.Date,
                        Cost = manifest.Cost
                    };
                    _context.Add(newDaily);
                }
                else
                {
                    newDaily.Cost += manifest.Cost;
                    _context.Update(newDaily);
                }
            }

            _context.SaveChanges();

            return true;
        }

        public bool DeleteManifest(string ID)
        {
            var manifest = _context.Manifests.SingleOrDefault(x => x.ID == ID);
            if (manifest == null)
            {
                throw new BusinessException((int)ErrorCode.NotFound, "该消费明细已删除");
            }

            _context.Remove(manifest);

            var daily = _context.Dailys.FirstOrDefault(x => x.Date.Date == manifest.Date.Date);
            if (_context.Manifests.Any(x => x.Date.Date == manifest.Date.Date))
            {
                daily.Cost -= manifest.Cost;
                _context.Update(daily);
            }
            else
            {
                _context.Remove(daily);
            }

            _context.SaveChanges();

            return true;
        }
    }
}

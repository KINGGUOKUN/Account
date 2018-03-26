using Account.Entity;
using Account.Infrustructure.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Repository.Contract
{
    public interface IManifestRepository : IRepository<Manifest>
    {
        Task<PaginatedList<Manifest>> GetManifests(DateTime start, DateTime end, int pageIndex, int pageSize);
    }
}

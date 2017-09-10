using Account.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Service.Contract
{
    public interface IManifestService : IService
    {
        Task<PaginatedList<Manifest>> GetManifests(DateTime start, DateTime end, int pageIndex, int pageSize);

        Manifest GetManifestById(string ID);

        Manifest AddManifest(Manifest manifest);

        bool UpdateManifest(Manifest manifest);

        bool DeleteManifest(string ID);
    }
}

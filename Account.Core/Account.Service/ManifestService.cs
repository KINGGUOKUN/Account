using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Account.Entity;
using Account.Service.Contract;
using Account.Repository.Contract;

namespace Account.Service
{
    public class ManifestService : IManifestService
    {
        private readonly IManifestRepository _manifestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ManifestService(IManifestRepository manifestRepository, IUnitOfWork unitOfWork)
        {
            _manifestRepository = manifestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<Manifest>> GetManifests(DateTime start, DateTime end, int pageIndex, int pageSize)
        {
            var pagedList = await _manifestRepository.GetManifests(start, end, pageIndex, pageSize);

            if (pageSize * (pageIndex - 1) >= pagedList.Count)
            {
                pageIndex = (int)Math.Ceiling(((double)pagedList.Count) / pageSize);
                pagedList = await _manifestRepository.GetManifests(start, end, pageIndex, pageSize);
            }

            return pagedList;
        }

        public Manifest GetManifestById(string ID)
        {
            return _manifestRepository.GetManifestById(ID);
        }

        public Manifest AddManifest(Manifest manifest)
        {
            return _manifestRepository.AddManifest(manifest);
        }

        public bool DeleteManifest(string ID)
        {
            return _manifestRepository.DeleteManifest(ID);
        }

        public bool UpdateManifest(Manifest manifest)
        {
            return _manifestRepository.UpdateManifest(manifest);
        }
    }
}

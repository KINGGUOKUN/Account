using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Account.Entity;
using Account.Service.Contract;
using Account.Repository.Contract;
using System.Linq;
using Account.Infrustructure.Contract;

namespace Account.Service
{
    public class ManifestService : IManifestService
    {
        private readonly IManifestRepository _manifestRepository;
        private readonly IDailyRepository _dailyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ManifestService(IManifestRepository manifestRepository,
            IDailyRepository dailyRepository,
            IUnitOfWork unitOfWork)
        {
            _manifestRepository = manifestRepository;
            _dailyRepository = dailyRepository;
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
            return _manifestRepository.GetByID(ID);
        }

        public Manifest AddManifest(Manifest manifest)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                _manifestRepository.Insert(manifest);
                var daily = _dailyRepository.Get(x => x.Date.Date == manifest.Date.Date).FirstOrDefault();
                if (daily != null)
                {
                    daily.Cost += manifest.Cost;
                    _dailyRepository.Update(daily);
                }
                else
                {
                    daily = new Daily
                    {
                        ID = Guid.NewGuid().ToString(),
                        Date = manifest.Date,
                        Cost = manifest.Cost
                    };
                    _dailyRepository.Insert(daily);
                }

                _unitOfWork.CommitTransaction();

                return manifest;
            }
            catch(Exception e)
            {
                _unitOfWork.RollbackTransaction();
                throw e;
            }           
        }

        public void DeleteManifest(string ID)
        {
            _manifestRepository.Delete(ID);
            _manifestRepository.Save();
        }

        public void UpdateManifest(Manifest manifest)
        {
            _manifestRepository.Update(manifest);
            _manifestRepository.Save();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Account.Entity;
using Account.Service.Contract;
using Account.Repository.Contract;

namespace Account.Service
{
    public class DailyService : IDailyService
    {
        private readonly IDailyRepository _dailyRepository;

        public DailyService(IDailyRepository dailyRepository)
        {
            _dailyRepository = dailyRepository;
        }

        public async Task<PaginatedList<Daily>> GetDailys(DateTime start, DateTime end, int pageIndex, int pageSize)
        {
            var pagedList = await _dailyRepository.GetDailys(start, end, pageIndex, pageSize);

            if (pageSize * (pageIndex - 1) >= pagedList.Count)
            {
                pageIndex = (int)Math.Ceiling(((double)pagedList.Count) / pageSize);
                pagedList = await _dailyRepository.GetDailys(start, end, pageIndex, pageSize);
            }

            return pagedList;
        }
    }
}

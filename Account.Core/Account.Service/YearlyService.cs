using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Account.Entity;
using Account.Service.Contract;
using Account.Repository.Contract;

namespace Account.Service
{
    public class YearlyService : IYearlyService
    {
        //private readonly IYearlyRepository _yearlyRepository;
        private readonly IDailyRepository _dailyRepository;

        public YearlyService(IDailyRepository  dailyRepository)
        {
            _dailyRepository = dailyRepository;
        }

        public async Task<PaginatedList<Yearly>> GetYearlys(int start, int end, int pageIndex, int pageSize)
        {
            var pagedList = await _dailyRepository.GetYearlys(start, end, pageIndex, pageSize);

            if (pageSize * (pageIndex - 1) >= pagedList.Count)
            {
                pageIndex = (int)Math.Ceiling(((double)pagedList.Count) / pageSize);
                pagedList = await _dailyRepository.GetYearlys(start, end, pageIndex, pageSize);
            }

            return pagedList;
        }
    }
}

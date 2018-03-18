using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Account.Entity;
using Account.Service.Contract;
using Account.Repository.Contract;

namespace Account.Service
{
    public class MonthlyService : IMonthlyService
    {
        //private readonly IMontylyRepository _monthlyRepository;
        private readonly IDailyRepository _dailyRepository;

        public MonthlyService(IDailyRepository dailyRepository)
        {
            _dailyRepository = dailyRepository;
        }

        public async Task<PaginatedList<Monthly>> GetMonthlys(string start, string end, int pageIndex, int pageSize)
        {
            var pagedList = await _dailyRepository.GetMonthlys(start, end, pageIndex, pageSize);

            if (pageSize * (pageIndex - 1) >= pagedList.Count)
            {
                pageIndex = (int)Math.Ceiling(((double)pagedList.Count) / pageSize);
                pagedList = await _dailyRepository.GetMonthlys(start, end, pageIndex, pageSize);
            }

            return pagedList;
        }
    }
}

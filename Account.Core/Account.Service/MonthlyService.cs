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
        private readonly IMontylyRepository _monthlyRepository;

        public MonthlyService(IMontylyRepository monthlyRepository)
        {
            _monthlyRepository = monthlyRepository;
        }

        public async Task<PaginatedList<Monthly>> GetMonthlys(string start, string end, int pageIndex, int pageSize)
        {
            var pagedList = await _monthlyRepository.GetMonthlys(start, end, pageIndex, pageSize);

            if (pageSize * (pageIndex - 1) >= pagedList.Count)
            {
                pageIndex = (int)Math.Ceiling(((double)pagedList.Count) / pageSize);
                pagedList = await _monthlyRepository.GetMonthlys(start, end, pageIndex, pageSize);
            }

            return pagedList;
        }
    }
}

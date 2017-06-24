using Account.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Repository.Contract
{
    public interface IDailyRepository
    {
        Task<PaginatedList<Daily>> GetDailys(DateTime start, DateTime end, int pageIndex, int pageSize);
    }
}

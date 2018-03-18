using Account.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Repository.Contract
{
    public interface IDailyRepository : IRepository<Daily>
    {
        Task<PaginatedList<Daily>> GetDailys(DateTime start, DateTime end, int pageIndex, int pageSize);

        Task<PaginatedList<Monthly>> GetMonthlys(string start, string end, int pageIndex, int pageSize);

        Task<PaginatedList<Yearly>> GetYearlys(int start, int end, int pageIndex, int pageSize);
    }
}

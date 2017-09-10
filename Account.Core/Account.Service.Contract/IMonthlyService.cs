using Account.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Service.Contract
{
    public interface IMonthlyService : IService
    {
        Task<PaginatedList<Monthly>> GetMonthlys(string start, string end, int pageIndex, int pageSize);
    }
}

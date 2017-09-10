using Account.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Service.Contract
{
    public interface IYearlyService : IService
    {
        Task<PaginatedList<Yearly>> GetYearlys(int start, int end, int pageIndex, int pageSize);
    }
}

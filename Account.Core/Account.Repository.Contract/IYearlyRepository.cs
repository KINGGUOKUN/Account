using Account.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Repository.Contract
{
    public interface IYearlyRepository : IRepository
    {
        Task<PaginatedList<Yearly>> GetYearlys(int start, int end, int pageIndex, int pageSize);
    }
}

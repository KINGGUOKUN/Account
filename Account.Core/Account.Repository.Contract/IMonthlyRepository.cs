using Account.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Repository.Contract
{
    public interface IMontylyRepository 
    {
        Task<PaginatedList<Monthly>> GetMonthlys(string start, string end, int pageIndex, int pageSize);
    }
}

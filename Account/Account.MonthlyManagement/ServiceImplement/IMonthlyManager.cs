using Account.MonthlyManagement.Entities;
using System.Collections.Generic;

namespace Account.MonthlyManagement.ServiceImplement
{
    public interface IMonthlyManager
    {
        List<Monthly> GetMonthlys(string start, string end);
    }
}

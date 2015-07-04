using Account.YearlyManagement.Entities;
using System.Collections.Generic;

namespace Account.YearlyManagement.ServiceImplement
{
    public interface IYearlyManager
    {
        List<Yearly> GetYearlys(string start, string end);
    }
}

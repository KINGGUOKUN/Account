using Account.Entity;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.DAL
{
    public class MonthlyDAL
    {
        #region Private Fields

        private readonly IDatabase _database = DatabaseFactory.Instance.CreateDatabase();

        #endregion

        #region Public Methods

        public IEnumerable<Monthly> GetMonthlys(string start, string end)
        {
            string sql = @"SELECT NEWID() ID, DATENAME(YEAR, DATE) + '-' + DATENAME(MONTH, DATE) MONTH, SUM(COST) COST
                           FROM DAILY
                           WHERE DATENAME(YEAR, DATE) + '-' + DATENAME(MONTH, DATE) BETWEEN @START AND @END
                           GROUP BY DATENAME(YEAR, DATE) + '-' + DATENAME(MONTH, DATE)
                           ORDER BY DATENAME(YEAR, DATE) + '-' + DATENAME(MONTH, DATE) ASC";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("START", start);
            parameters.Add("END", end);

            return _database.GetList<Monthly>(sql, parameters);
        }

        #endregion
    }
}

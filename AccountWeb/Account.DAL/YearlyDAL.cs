using Account.Common;
using Account.Entity;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.DAL
{
    public class YearlyDAL
    {
        #region Private Fields

        private readonly IDatabase _database = DatabaseFactory.CreateDatabase();

        #endregion

        #region Public Methods

        public IEnumerable<Yearly> GetYearlys(string start, string end)
        {
            string sql = @"SELECT NEWID() ID, DATENAME(YEAR, DATE) YEAR, SUM(COST) COST
                           FROM DAILY
                           WHERE DATENAME(YEAR, DATE) BETWEEN @START AND @END
                           GROUP BY DATENAME(YEAR, DATE)
                           ORDER BY DATENAME(YEAR, DATE) ASC";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"START", start },
                { "END", end}
            };

            return _database.GetList<Yearly>(sql, parameters);
        }

        #endregion
    }
}

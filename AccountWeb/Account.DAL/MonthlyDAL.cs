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
    public class MonthlyDAL
    {
        #region Private Fields

        private readonly IDatabase _database = DatabaseFactory.CreateDatabase();

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

        public IEnumerable<Monthly> GetMonthlys(string start, string end, int pageIndex, int pageSize, ref int count)
        {
            string sql = @"SELECT NEWID() ID, ROW_NUMBER() OVER(ORDER BY CONVERT(CHAR(7), DATE, 120)) RowNum, CONVERT(CHAR(7), DATE, 120) MONTH, SUM(COST) COST
	                             FROM DAILY
	                             WHERE CONVERT(CHAR(7), DATE, 120) BETWEEN @START AND @END
	                             GROUP BY CONVERT(CHAR(7), DATE, 120)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("START", start);
            parameters.Add("END", end);
            var obj = _database.ExecuteScalar(string.Format("SELECT COUNT(*) FROM ({0}) V", sql), parameters);
            if (obj != null)
            {
                count = int.Parse(obj.ToString());
            }
            if (count == 0)
            {
                return null;
            }
            sql = string.Format(@"SELECT v.ID, v.MONTH, v.COST
                                  FROM ({0}) V
                                  WHERE v.RowNum BETWEEN @startRowNum AND @endRowNum", sql);
            parameters.Add("@startRowNum", pageSize * (pageIndex - 1) + 1);
            parameters.Add("@endRowNum", pageSize * pageIndex);

            return _database.GetList<Monthly>(sql, parameters);
        }

        #endregion
    }
}

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

        public IEnumerable<Yearly> GetYearlys(string start, string end, int pageIndex, int pageSize, ref int count)
        {
            string sql = @"SELECT NEWID() ID, ROW_NUMBER() OVER(ORDER BY DATENAME(YEAR, DATE)) RowNum, 
                           DATENAME(YEAR, DATE) YEAR, SUM(COST) COST
	                       FROM DAILY
	                       WHERE DATENAME(YEAR, DATE) BETWEEN @START AND @END
	                       GROUP BY DATENAME(YEAR, DATE)";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@START", start},
                { "@END", end}
            };
            var obj = _database.ExecuteScalar(string.Format("SELECT COUNT(*) FROM ({0}) V", sql), parameters);
            if (obj != null)
            {
                count = int.Parse(obj.ToString());
            }
            if (count == 0)
            {
                return null;
            }

            sql = string.Format(@"SELECT v.ID, v.YEAR, v.COST
                                  FROM ({0}) V
                                  WHERE v.RowNum BETWEEN @startRowNum AND @endRowNum", sql);
            parameters.Add("@startRowNum", pageSize * (pageIndex - 1) + 1);
            parameters.Add("@endRowNum", pageSize * pageIndex);

            return _database.GetList<Yearly>(sql, parameters);
        }

        #endregion
    }
}

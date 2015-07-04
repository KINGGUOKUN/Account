using Account.YearlyManagement.Entities;
using GuoKun.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Account.YearlyManagement.ServiceImplement
{
    public class YearlyService
    {
        #region Private Fields

        private Database _database = null;

        #endregion

        #region Constructors

        public YearlyService()
        {
            _database = DatabaseFactory.CreateDatabase();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 获取日消费清单
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Yearly> GetYearlys(string start, string end)
        {
            List<Yearly> result = null;
            DataTable dt = new DataTable();
            string sql = @"SELECT NEWID() ID, DATENAME(YEAR, DATE) YEAR, SUM(COST) COST
                           FROM DAILY
                           WHERE DATENAME(YEAR, DATE) BETWEEN @START AND @END
                           GROUP BY DATENAME(YEAR, DATE)
                           ORDER BY DATENAME(YEAR, DATE) ASC";
            using (DbCommand cmd = _database.GetSqlStringCommand(sql))
            {
                _database.AddInParameter(cmd, "@START", DbType.String, start);
                _database.AddInParameter(cmd, "@END", DbType.String, end);
                using (IDataReader reader = _database.ExecuteReader(cmd))
                {
                    dt.Load(reader);
                }
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                result = dt.ToList<Yearly>();
            }

            return result;
        }

        #endregion
    }
}

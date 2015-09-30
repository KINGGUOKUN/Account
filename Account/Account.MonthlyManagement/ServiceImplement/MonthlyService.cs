using Account.MonthlyManagement.Entities;
using GuoKun.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Account.MonthlyManagement.ServiceImplement
{
    public class MonthlyService
    {
        #region Private Fields

        private Database _database = null;

        #endregion

        #region Constructors

        public MonthlyService()
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
        public List<Monthly> GetMonthlys(string start, string end)
        {
            List<Monthly> result = null;
            DataTable dt = new DataTable();
            string sql = @"SELECT NEWID() ID, DATENAME(YEAR, DATE) + '-' + DATENAME(MONTH, DATE) MONTH, SUM(COST) COST
                           FROM DAILY
                           WHERE DATENAME(YEAR, DATE) + '-' + DATENAME(MONTH, DATE) BETWEEN @START AND @END
                           GROUP BY DATENAME(YEAR, DATE) + '-' + DATENAME(MONTH, DATE)
                           ORDER BY DATENAME(YEAR, DATE) + '-' + DATENAME(MONTH, DATE) ASC";
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
                result = dt.ToList<Monthly>();
            }

            return result;
        }

        #endregion
    }
}

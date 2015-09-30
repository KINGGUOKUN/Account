using Account.DailyManagement.Entities;
using GuoKun.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Account.DailyManagement.ServiceImplement
{
    public class DailyService
    {
        #region Private Fields

        private Database _database = null;

        #endregion

        #region Constructors

        public DailyService()
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
        public List<Daily> GetDailys(DateTime start, DateTime end)
        {
            List<Daily> result = null;
            DataTable dt = new DataTable();
            string sql = @"SELECT D.* FROM DAILY D WHERE D.DATE BETWEEN @START AND @END ORDER BY D.DATE ASC";
            using (DbCommand cmd = _database.GetSqlStringCommand(sql))
            {
                _database.AddInParameter(cmd, "@START", DbType.Date, start);
                _database.AddInParameter(cmd, "@END", DbType.Date, end);
                using (IDataReader reader = _database.ExecuteReader(cmd))
                {
                    dt.Load(reader);
                }
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                result = dt.ToList<Daily>();
            }

            return result;
        }

        #endregion
    }
}

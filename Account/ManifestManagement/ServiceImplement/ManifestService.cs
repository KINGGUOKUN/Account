using Account.ManifestManagement.Entities;
using GuoKun.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Account.ManifestManagement.ServiceImplement
{
    /// <summary>
    /// 日消费清单服务
    /// </summary>
    public class ManifestService
    {
        private Database _database = null;

        public ManifestService()
        {
            _database = DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// 按起止日期获取消费明细
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Manifest> GetManifest(DateTime begin, DateTime end)
        {
            List<Manifest> result = null;
            DataTable dt = new DataTable();
            string sql = @"SELECT * 
                           FROM MANIFEST M 
                           WHERE M.DATE BETWEEN @BEGIN AND @END 
                           ORDER BY M.DATE ASC, M.COST ASC";
            using(DbCommand cmd = _database.GetSqlStringCommand(sql))
            {
                _database.AddInParameter(cmd, "@BEGIN", DbType.Date, begin);
                _database.AddInParameter(cmd, "@END", DbType.Date, end);
                using(IDataReader reader = _database.ExecuteReader(cmd))
                {
                    dt.Load(reader);
                }
            }
            if(dt != null && dt.Rows.Count > 0)
            {
                result = dt.ToList<Manifest>();
            }

            return result;
        }

        /// <summary>
        /// 添加消费明细
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns></returns>
        public int AddManifest(Manifest manifest)
        {
            int result = -1;
            string sql = @"INSERT INTO MANIFEST VALUES(@ID, @DATE, @COST, @REMARK)";
            using(DbCommand cmd = _database.GetSqlStringCommand(sql))
            {
                _database.AddInParameter(cmd, "@ID", DbType.Guid, manifest.ID);
                _database.AddInParameter(cmd, "@DATE", DbType.DateTime, manifest.Date);
                _database.AddInParameter(cmd, "@COST", DbType.Decimal, manifest.Cost);
                _database.AddInParameter(cmd, "@REMARK", DbType.String, manifest.Remark);
                result = _database.ExecuteNonQuery(cmd);
            }
            
            return result;
        }

        /// <summary>
        /// 更新消费明细
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns></returns>
        public int UpdateManifest(Manifest manifest)
        {
            int result = -1;
            string sql = @"UPDATE MANIFEST SET DATE = @DATE, COST = @COST, REMARK = @REMARK WHERE ID = @ID";
            using(DbCommand cmd = _database.GetSqlStringCommand(sql))
            {
                _database.AddInParameter(cmd, "@DATE", DbType.DateTime, manifest.Date);
                _database.AddInParameter(cmd, "@COST", DbType.Decimal, manifest.Cost);
                _database.AddInParameter(cmd, "@REMARK", DbType.String, manifest.Remark);
                _database.AddInParameter(cmd, "@ID", DbType.Guid, manifest.ID);
                result = _database.ExecuteNonQuery(cmd);
            }

            return result;
        }

        /// <summary>
        /// 删除消费明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteManifest(Guid id)
        {
            int result = -1;
            string sql = @"DELETE FROM MANIFEST WHERE ID = @ID";
            using(DbCommand cmd = _database.GetSqlStringCommand(sql))
            {
                _database.AddInParameter(cmd, "@ID", DbType.Guid, id);
                result = _database.ExecuteNonQuery(cmd);
            }

            return result;
        }

    }
}

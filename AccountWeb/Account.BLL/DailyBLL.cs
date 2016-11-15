using Account.DAL;
using Account.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.BLL
{
    /// <summary>
    /// 日消费清单
    /// </summary>
    public class DailyBLL
    {
        #region Private Fields

        private readonly DailyDAL _dal = new DailyDAL();

        #endregion

        #region Public Methods

        /// <summary>
        /// 获取日消费清单
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IEnumerable<Daily> GetDailys(DateTime start, DateTime end)
        {
            return _dal.GetDailys(start, end);
        }

        /// <summary>
        /// 获取日消费清单
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public dynamic GetDailys(DateTime start, DateTime end, int pageIndex, int pageSize)
        {
            int count = 0;
            var manifests = _dal.GetDailys(start, end, pageIndex, pageSize, ref count);

            if (pageSize * (pageIndex - 1) >= count)
            {
                pageIndex = (int)Math.Ceiling(((double)count) / pageSize);
                manifests = _dal.GetDailys(start, end, pageIndex, pageSize, ref count);
            }

            return new
            {
                pageIndex = pageIndex,
                count,
                data = manifests
            };
        }

        #endregion
    }
}

using Account.BLL;
using Account.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Account.Controllers
{
    /// <summary>
    /// 日消费清单
    /// </summary>
    [RoutePrefix("api/Dailys")]
    public class DailyController : ApiController
    {
        #region Private Fields

        private readonly DailyBLL _bll = new DailyBLL();

        #endregion

        #region Public Methods

        /// <summary>
        /// 获取日消费清单
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [Route("")]
        public IEnumerable<Daily> GetDailys(DateTime start, DateTime end)
        {
            return _bll.GetDailys(start, end);
        }

        /// <summary>
        /// 获取日消费清单
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [Route("paged"), HttpGet]
        public dynamic GetDailys(DateTime start, DateTime end, int pageIndex, int pageSize)
        {
            return _bll.GetDailys(start, end, pageIndex, pageSize);
        }

        #endregion
    }
}

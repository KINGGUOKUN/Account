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
    [RoutePrefix("api/Monthly")]
    public class MonthlyController : ApiController
    {
        #region Private Fields

        private readonly MonthlyBLL _bll = new MonthlyBLL();

        #endregion

        #region Public Methods

        [Route("")]
        public IEnumerable<Monthly> GetMonthlys(string start, string end)
        {
            return _bll.GetMonthlys(start, end);
        }

        #endregion
    }
}

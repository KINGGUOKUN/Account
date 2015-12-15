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
    [RoutePrefix("api/Yearly")]
    public class YearlyController : ApiController
    {
        #region Private Fields

        private readonly YearlyBLL _bll = new YearlyBLL();

        #endregion

        #region Public Methods

        [Route("")]
        public IEnumerable<Yearly> GetYearlys(string start, string end)
        {
            return _bll.GetYearlys(start, end);
        }

        #endregion
    }
}

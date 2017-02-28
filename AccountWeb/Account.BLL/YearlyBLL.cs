using Account.DAL;
using Account.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.BLL
{
    public class YearlyBLL
    {
        #region Private Fields

        private readonly YearlyDAL _dal = new YearlyDAL();

        #endregion

        #region Public Methods

        public IEnumerable<Yearly> GetYearlys(string start, string end)
        {
            return _dal.GetYearlys(start, end);
        }

        public dynamic GetYearlys(string start, string end, int pageIndex, int pageSize)
        {
            int count = 0;
            var monthlys = _dal.GetYearlys(start, end, pageIndex, pageSize, ref count);

            if (pageSize * (pageIndex - 1) >= count)
            {
                pageIndex = (int)Math.Ceiling(((double)count) / pageSize);
                monthlys = _dal.GetYearlys(start, end, pageIndex, pageSize, ref count);
            }

            return new
            {
                pageIndex = pageIndex,
                count,
                data = monthlys
            };
        }

        #endregion
    }
}

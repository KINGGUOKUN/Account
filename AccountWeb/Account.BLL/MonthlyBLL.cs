using Account.DAL;
using Account.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.BLL
{
    public class MonthlyBLL
    {
        #region Private Fields

        private readonly MonthlyDAL _dal = new MonthlyDAL();

        #endregion

        #region Public Methods

        public IEnumerable<Monthly> GetMonthlys(string start, string end)
        {
            return _dal.GetMonthlys(start, end);
        }

        public dynamic GetMonthlys(string start, string end, int pageIndex, int pageSize)
        {
            int count = 0;
            var monthlys = _dal.GetMonthlys(start, end, pageIndex, pageSize, ref count);

            if (pageSize * (pageIndex - 1) >= count)
            {
                pageIndex = (int)Math.Ceiling(((double)count) / pageSize);
                monthlys = _dal.GetMonthlys(start, end, pageIndex, pageSize, ref count);
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

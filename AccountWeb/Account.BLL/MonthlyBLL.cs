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

        #endregion
    }
}

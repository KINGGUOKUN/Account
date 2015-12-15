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

        #endregion
    }
}

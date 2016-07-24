using Account.Common;
using Account.Entity;
using DapperExtensions;
using System;
using System.Collections.Generic;

namespace Account.DAL
{
    public class DailyDAL
    {
        #region Private Fields

        private readonly IDatabase _database = DatabaseFactory.CreateDatabase();

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
            IPredicate[] predicates = new IPredicate[]
            {
                Predicates.Field<Daily>(x => x.Date, Operator.Ge, start),
                Predicates.Field<Daily>(x => x.Date, Operator.Le, end)
            };
            IList<ISort> sorts = new List<ISort>()
            {
                Predicates.Sort<Daily>(x => x.Date)
            };

            return _database.GetList<Daily>(Predicates.Group(GroupOperator.And, predicates), sorts);
        }

        #endregion
    }
}

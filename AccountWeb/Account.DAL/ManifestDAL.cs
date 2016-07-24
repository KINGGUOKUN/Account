using Account.Common;
using Account.Entity;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.DAL
{
    public class ManifestDAL 
    {
        #region Private Fields

        private IDatabase _database = null;

        #endregion

        #region Constructors

        public ManifestDAL()
        {
            _database = DatabaseFactory.CreateDatabase();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 按起止日期获取消费明细
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IEnumerable<Manifest> GetManifest(DateTime begin, DateTime end)
        {
            IPredicate[] predicates = new IPredicate[]
            {
                Predicates.Field<Manifest>(x => x.Date, Operator.Ge, begin),
                Predicates.Field<Manifest>(x => x.Date, Operator.Le, end)
            };
            IList<ISort> sorts = new List<ISort>()
            {
                Predicates.Sort<Manifest>(x => x.Date),
                Predicates.Sort<Manifest>(x => x.Cost)
            };

            return _database.GetList<Manifest>(Predicates.Group(GroupOperator.And, predicates), sorts);
        }

        /// <summary>
        /// 添加消费明细
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns></returns>
        public Manifest AddManifest(Manifest manifest)
        {
            _database.Insert(manifest);

            return manifest;
        }

        /// <summary>
        /// 修改消费明细
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns></returns>
        public bool UpdateManifest(Manifest manifest)
        {
            return _database.Update(manifest);
        }

        /// <summary>
        /// 删除消费明细
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DeleteManifest(string ID)
        {
            return _database.Delete<Manifest>(Predicates.Field<Manifest>(x => x.ID, Operator.Eq, ID));
        }

        #endregion
    }
}

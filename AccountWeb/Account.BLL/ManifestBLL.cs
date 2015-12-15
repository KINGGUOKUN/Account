using Account.DAL;
using Account.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.BLL
{
    public class ManifestBLL
    {
        #region Private Fields

        private readonly ManifestDAL _dal = new ManifestDAL();

        #endregion

        #region Constructors

        public ManifestBLL()
        {
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
            return _dal.GetManifest(begin, end);
        }

        /// <summary>
        /// 添加消费明细
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns></returns>
        public Manifest AddManifest(Manifest manifest)
        {
            return _dal.AddManifest(manifest);
        }

        /// <summary>
        /// 修改消费明细
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns></returns>
        public bool UpdateManifest(Manifest manifest)
        {
            return _dal.UpdateManifest(manifest);
        }

        /// <summary>
        /// 删除消费明细
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DeleteManifest(string ID)
        {
            return _dal.DeleteManifest(ID);
        }

        #endregion
    }
}

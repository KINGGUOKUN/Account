using Account.ManifestManagement.Entities;
using GuoKun.Configuration;
using log4net;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Account.ManifestManagement.ServiceImplement
{
    /// <summary>
    /// 服务接口实现(BLL)
    /// </summary>
    public class ManifestManager : IManifestManager
    {
        #region Private Fields

        private readonly ILog _log;
        private readonly ManifestService _service;

        #endregion

        #region Constructors
        public ManifestManager()
        {
            _log = UnityContainerFactory.GetUnityContainer().Resolve<ILog>();
            _service = new ManifestService();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 获取消费明细
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Manifest> GetManifest(DateTime begin, DateTime end)
        {
            List<Manifest> result = null;
            try
            {
                result = _service.GetManifest(begin, end);
            }
            catch(Exception e)
            {
                _log.Error("GetManifest(DateTime begin, DateTime end)", e);
                throw new Exception("获取消费明细出错");
            }

            return result;
        }

        /// <summary>
        /// 添加消费明细
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns></returns>
        public bool AddManifest(Manifest manifest)
        {
            bool result = false;
            try
            {
                int count = _service.AddManifest(manifest);
                if(count > 0)
                {
                    result = true;
                }
            }
            catch(Exception e)
            {
                _log.Error("AddManifest(Manifest manifest)", e);
            }

            return result;
        }

        /// <summary>
        /// 更新消费明细
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns></returns>
        public bool UpdateManifest(Manifest manifest)
        {
            bool result = false;
            try
            {
                int count = _service.UpdateManifest(manifest);
                if (count > 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                _log.Error("UpdateManifest(Manifest manifest)", e);
            }

            return result;
        }

        /// <summary>
        /// 删除消费明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteManifest(Guid id)
        {
            bool result = false;
            try
            {
                int count = _service.DeleteManifest(id);
                if (count > 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                _log.Error("DeleteManifest(Guid id)", e);
            }

            return result;
        }

        #endregion
    }
}

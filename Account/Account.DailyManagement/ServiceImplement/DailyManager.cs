using Account.DailyManagement.Entities;
using GuoKun.Configuration;
using log4net;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Account.DailyManagement.ServiceImplement
{
    public class DailyManager : IDailyManager
    {
        #region Private Fields

        private readonly ILog _log;
        private readonly DailyService _service;

        #endregion

        #region Constructors
        #endregion

        public DailyManager()
        {
            _log = UnityContainerFactory.GetUnityContainer().Resolve<ILog>();
            _service = new DailyService();
        }

        #region Public Methods
        
        /// <summary>
        /// 获取日消费清单
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Daily> GetDailys(DateTime start, DateTime end)
        {
            List<Daily> result = null;
            try
            {
                result = _service.GetDailys(start, end);
            }
            catch(Exception e)
            {
                _log.Error("GetDailys(DateTime start, DateTime end)", e);
                throw new Exception("获取日消费清单出错");
            }

            return result;
        }

        #endregion

    }
}

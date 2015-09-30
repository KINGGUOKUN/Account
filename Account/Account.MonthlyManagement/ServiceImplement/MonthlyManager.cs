using Account.MonthlyManagement.Entities;
using GuoKun.Configuration;
using log4net;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Account.MonthlyManagement.ServiceImplement
{
    public class MonthlyManager : IMonthlyManager
    {
        #region Private Fields

        private readonly ILog _log;
        private readonly MonthlyService _service;

        #endregion

        #region Constructors
        #endregion

        public MonthlyManager()
        {
            _log = UnityContainerFactory.GetUnityContainer().Resolve<ILog>();
            _service = new MonthlyService();
        }

        #region Public Methods

        public List<Monthly> GetMonthlys(string start, string end)
        {
            List<Monthly> result = null;
            try
            {
                result = _service.GetMonthlys(start, end);
            }
            catch (Exception e)
            {
                _log.Error("GetMonthlys(string start, string end)", e);
                throw new Exception("获取月消费清单出错");
            }

            return result;
        }

        #endregion
    }
}

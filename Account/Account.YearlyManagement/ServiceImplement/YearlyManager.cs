using Account.YearlyManagement.Entities;
using GuoKun.Configuration;
using log4net;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Account.YearlyManagement.ServiceImplement
{
    public class YearlyManager : IYearlyManager
    {
        #region Private Fields

        private readonly ILog _log;
        private readonly YearlyService _service;

        #endregion

        #region Constructors

        public YearlyManager()
        {
            _log = UnityContainerFactory.GetUnityContainer().Resolve<ILog>();
            _service = new YearlyService();
        }

        #endregion

        #region Public Methods

        public List<Yearly> GetYearlys(string start, string end)
        {
            List<Yearly> result = null;
            try
            {
                result = _service.GetYearlys(start, end);
            }
            catch (Exception e)
            {
                _log.Error("GetYearlys(string start, string end)", e);
                throw new Exception("获取月消费清单出错");
            }

            return result;
        }

        #endregion
    }
}

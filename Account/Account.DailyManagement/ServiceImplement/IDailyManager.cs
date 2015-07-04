using Account.DailyManagement.Entities;
using System;
using System.Collections.Generic;

namespace Account.DailyManagement.ServiceImplement
{
    /// <summary>
    /// 日消费清单管理接口
    /// </summary>
    public interface IDailyManager
    {
        List<Daily> GetDailys(DateTime start, DateTime end);
    }
}

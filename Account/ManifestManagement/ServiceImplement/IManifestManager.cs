using Account.ManifestManagement.Entities;
using System;
using System.Collections.Generic;

namespace Account.ManifestManagement.ServiceImplement
{
    public interface IManifestManager
    {
        /// <summary>
        /// 根据起始结束时间获取清单列表
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        List<Manifest> GetManifest(DateTime begin, DateTime end);

        /// <summary>
        /// 添加消费清单
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns></returns>
        bool AddManifest(Manifest manifest);

        /// <summary>
        /// 修改消费清单
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns></returns>
        bool UpdateManifest(Manifest manifest);

        /// <summary>
        /// 删除消费清单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteManifest(Guid id);
    }
}

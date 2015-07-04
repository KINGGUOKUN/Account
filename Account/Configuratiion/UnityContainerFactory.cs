using Microsoft.Practices.Unity;
using System;

namespace GuoKun.Configuration
{
    /// <summary>
    /// 依赖注入容器工厂
    /// </summary>
    public static class UnityContainerFactory
    {
        private static IUnityContainer _container;

        /// <summary>
        /// 将构造好的容器注册进容器工厂
        /// </summary>
        /// <param name="container"></param>
        public static void RegisterUnityContainer(IUnityContainer container)
        {
            if(container == null)
            {
                throw new ArgumentNullException("container不能为空");
            }
            _container = container;
        }

        /// <summary>
        /// 取得依赖注入容器
        /// </summary>
        /// <returns></returns>
        public static IUnityContainer GetUnityContainer()
        {
            return _container;
        }
    }
}

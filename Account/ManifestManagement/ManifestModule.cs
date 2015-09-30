using Account.Entity;
using Account.ManifestManagement.ServiceImplement;
using Account.ManifestManagement.View;
using GuoKun.Configuration;
using Microsoft.Practices.Unity;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using System.ComponentModel.Composition;

namespace Account.ManifestManagement
{
    /// <summary>
    /// 日常消费清单模块
    /// </summary>
    [ModuleExport(typeof(ManifestModule))]
    public class ManifestModule : IModule
    {
        [Import]
        private IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        public ManifestModule()
        {
            _container = UnityContainerFactory.GetUnityContainer();
        }

        public void Initialize()
        {
            _container.RegisterType<IManifestManager, ManifestManager>();

            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, typeof(ManifestNavigationItemView));
            //_regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(ManifestManagementView));     
        }
    }
}

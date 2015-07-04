using Account.DailyManagement.ServiceImplement;
using Account.DailyManagement.View;
using Account.Entity;
using GuoKun.Configuration;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System.ComponentModel.Composition;

namespace Account.DailyManagement
{
    [ModuleExport(typeof(DailyModule))]
    public class DailyModule : IModule
    {
        [Import]
        private IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        public DailyModule()
        {
            _container = UnityContainerFactory.GetUnityContainer();
        }

        public void Initialize()
        {
            _container.RegisterType<IDailyManager, DailyManagement.ServiceImplement.DailyManager>();

            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, typeof(DailyNavigationItemView));
        }
    }
}

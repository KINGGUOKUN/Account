using Account.Entity;
using Account.MonthlyManagement.ServiceImplement;
using Account.MonthlyManagement.View;
using GuoKun.Configuration;
using Microsoft.Practices.Unity;
using Prism.Mef.Modularity;
using Prism.Modularity;
using Prism.Regions;
using System.ComponentModel.Composition;

namespace Account.MonthlyManagement
{
    [ModuleExport(typeof(MonthlyModule))]
    public class MonthlyModule : IModule
    {
        [Import]
        private IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        public MonthlyModule()
        {
            _container = UnityContainerFactory.GetUnityContainer();
        }

        public void Initialize()
        {
            _container.RegisterType<IMonthlyManager, MonthlyManager>();

            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, typeof(MonthlyNavigationItemView));
        }
    }
}

using Account.Entity;
using Account.YearlyManagement.ServiceImplement;
using Account.YearlyManagement.View;
using GuoKun.Configuration;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System.ComponentModel.Composition;

namespace Account.YearlyManagement
{
    [ModuleExport(typeof(YearlyModule))]
    public class YearlyModule : IModule
    {
        [Import]
        private IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        public YearlyModule()
        {
            _container = UnityContainerFactory.GetUnityContainer();
        }

        public void Initialize()
        {
            _container.RegisterType<IYearlyManager, YearlyManager>();

            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, typeof(YearlyNavigationItemView));
        }
    }
}

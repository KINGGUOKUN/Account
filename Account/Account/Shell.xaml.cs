using Account.Entity;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.ComponentModel.Composition;
using System.Windows;

namespace Account
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    [Export]
    public partial class Shell : Window, IPartImportsSatisfiedNotification
    {
        private const string ManifestModuleName = "ManifestModule";
        private readonly Uri _manifestUri = new Uri("ManifestManagementView", UriKind.Relative);

        public Shell()
        {
            InitializeComponent();
        }

        [Import(AllowRecomposition = false)]
        public IModuleManager ModuleManager;

        [Import(AllowRecomposition = false)]
        public IRegionManager RegionManager;

        public void OnImportsSatisfied()
        {
            this.ModuleManager.LoadModuleCompleted +=
                (s, e) =>
                {
                    if (e.ModuleInfo.ModuleName == ManifestModuleName)
                    {
                        this.RegionManager.RequestNavigate(
                            RegionNames.MainContentRegion,
                            _manifestUri);
                    }
                };
        }
    }
}

using Account.Entity;
using Prism.Regions;
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;

namespace Account.MonthlyManagement.View
{
    /// <summary>
    /// MonthlyNavigationItemView.xaml 的交互逻辑
    /// </summary>
    [Export]
    [ViewSortHint("03")]
    public partial class MonthlyNavigationItemView : UserControl, IPartImportsSatisfiedNotification
    {
        [Import]
        private IRegionManager _regionManager;
        private readonly Uri _monthlyUrl = new Uri("MonthlyManagementView", UriKind.Relative);

        public MonthlyNavigationItemView()
        {
            InitializeComponent();
        }

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            IRegion mainContentRegion = this._regionManager.Regions[RegionNames.MainContentRegion];
            if (mainContentRegion != null && mainContentRegion.NavigationService != null)
            {
                mainContentRegion.NavigationService.Navigated += this.MainContentRegion_Navigated;
            }
        }

        private void MainContentRegion_Navigated(object sender, RegionNavigationEventArgs e)
        {
            this.UpdateNavigationButtonState(e.Uri);
        }

        private void UpdateNavigationButtonState(Uri uri)
        {
            this.NavigateToMonthlyRadioButton.IsChecked = (uri == _monthlyUrl);
        }

        /// <summary>
        /// 日消费清单按钮点击事件
        /// 用于将主区域导航到日消费清单管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, _monthlyUrl);
        }
    }
}

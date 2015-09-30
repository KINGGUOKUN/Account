using Account.YearlyManagement.ViewModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Account.YearlyManagement.View
{
    /// <summary>
    /// YearlyManagementView.xaml 的交互逻辑
    /// </summary>
    [Export("YearlyManagementView")]
    public partial class YearlyManagementView : UserControl
    {
        public YearlyManagementView()
        {
            InitializeComponent();
        }

        [Import]
        public VMYearlyManagement ViewModel
        {
            get
            {
                return this.DataContext as VMYearlyManagement;
            }
            set
            {
                this.DataContext = value;
            }
        }

        private void dgYearly_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!this.IsMouseDoubleClickDataGridRow(sender, e))
            {
                return;
            }
            YearlyDetail detail = new YearlyDetail(this.ViewModel.SelectedItem);
            detail.Owner = Application.Current.MainWindow;
            detail.ShowDialog();
        }

        private bool IsMouseDoubleClickDataGridRow(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dependencyObject = e.OriginalSource as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is DataGridRow)
                {
                    return true;
                }
                else
                {
                    dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
                }
            }
            return false;
        }
    }
}

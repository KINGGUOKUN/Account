using Account.ManifestManagement.ViewModel;
using System;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Account.ManifestManagement.View
{
    /// <summary>
    /// ManifestManagement.xaml 的交互逻辑
    /// </summary>
    [Export("ManifestManagementView")]
    public partial class ManifestManagementView : UserControl
    {
        public ManifestManagementView()
        {
            InitializeComponent();
        }

        [Import]
        public VMManifestManagement ViewModel
        {
            get
            {
                return this.DataContext as VMManifestManagement;
            }
            set
            {
                this.DataContext = value;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string id = (sender as Button).Tag.ToString();
            this.ViewModel.SelectedItem = this.ViewModel.Manifests.Where(x => x.ID.ToString() == id).First();
            this.ViewModel.Update.Execute(null);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string id = (sender as Button).Tag.ToString();
            this.ViewModel.SelectedItem = this.ViewModel.Manifests.Where(x => x.ID.ToString() == id).First();
            this.ViewModel.Delete.Execute(null);
        }

        private void dgManifest_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!this.IsMouseDoubleClickDataGridRow(sender, e))
            {
                return;
            }
            this.ViewModel.Update.Execute(null);
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

    /// <summary>
    /// 前景色转换器(主要根据花销设置单元格字体颜色)
    /// </summary>
    [ValueConversion(typeof(decimal), typeof(string))]
    public class ForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "Black";
            decimal sourceValue = 0M;
            if (value != null && decimal.TryParse(value.ToString(), out sourceValue))
            {
                if (sourceValue >= 50)
                {
                    result = "Red";
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

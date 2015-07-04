using Account.MonthlyManagement.ViewModel;
using System;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Account.MonthlyManagement.View
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    [Export("MonthlyManagementView")]
    public partial class MonthlyManagementView : UserControl
    {
        public MonthlyManagementView()
        {
            InitializeComponent();
        }

        [Import]
        public VMMonthlyManagement ViewModel
        {
            get
            {
                return this.DataContext as VMMonthlyManagement;
            }
            set
            {
                this.DataContext = value;
            }
        }

        private void dgMonthly_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!this.IsMouseDoubleClickDataGridRow(sender, e))
            {
                return;
            }
            MonthlyDetail detail = new MonthlyDetail(this.ViewModel.SelectedItem);
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
                if (sourceValue >= 1500)
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

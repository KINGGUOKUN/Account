using Account.MonthlyManagement.ViewModel;
using System.Windows;

namespace Account.MonthlyManagement.View
{
    /// <summary>
    /// MonthlyDetail.xaml 的交互逻辑
    /// </summary>
    public partial class MonthlyDetail : Window
    {
        public MonthlyDetail(MonthlyModel model)
        {
            InitializeComponent();
            this.DataContext = model;
        }
    }
}

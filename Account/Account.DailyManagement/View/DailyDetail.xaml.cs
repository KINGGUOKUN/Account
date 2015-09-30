using Account.DailyManagement.ViewModel;
using System.Windows;

namespace Account.DailyManagement.View
{
    /// <summary>
    /// DailyDetail.xaml 的交互逻辑
    /// </summary>
    public partial class DailyDetail : Window
    {
        public DailyDetail(DailyModel model)
        {
            InitializeComponent();
            this.DataContext = model;
        }
    }
}

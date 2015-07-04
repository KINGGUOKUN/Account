using Account.YearlyManagement.ViewModel;
using System.Windows;

namespace Account.YearlyManagement.View
{
    /// <summary>
    /// YearlyDetail.xaml 的交互逻辑
    /// </summary>
    public partial class YearlyDetail : Window
    {
        public YearlyDetail(YearlyModel model)
        {
            InitializeComponent();
            this.DataContext = model;
        }
    }
}

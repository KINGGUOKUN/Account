using Account.Entity;
using Account.ManifestManagement.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;

namespace Account.ManifestManagement.View
{
    /// <summary>
    /// AddOrUpdateManifest.xaml 的交互逻辑
    /// </summary>
    public partial class AddOrUpdateManifest : Window
    {
        public event EventHandler<EntityEventArgs> HandleCompleted;

        public AddOrUpdateManifest(ManifestModel model)
        {
            InitializeComponent();
            this.ViewModel = new VMAddOrUpdateManifest(model);
            this.ViewModel.HandleCompleted += new EventHandler<EntityEventArgs>(ViewModel_HandleCompleted);
            if (model == null)
            {
                this.Title = "添加消费明细";
            }
            else
            {
                this.Title = "编辑消费明细";
            }
        }

        public VMAddOrUpdateManifest ViewModel
        {
            get
            {
                return this.DataContext as VMAddOrUpdateManifest;
            }
            set
            {
                this.DataContext = value;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Save.Execute(null);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ViewModel_HandleCompleted(object sender, EntityEventArgs e)
        {
            if (this.HandleCompleted != null)
            {
                this.HandleCompleted(this, e);
            }
            this.Close();
        }

        private void txtCost_KeyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtCost.SelectedText) && this.IsNumber(e.Key))
            {
                return;
            }
            string cost = this.txtCost.Text;
            if (!this.IsNumber(e.Key))
            {
                e.Handled = true;
                return;
            }
            if (e.Key == Key.OemPeriod || e.Key == Key.Decimal)
            {
                e.Handled = cost.IndexOf(".") != -1;
                return;
            }
            if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
            {
                e.Handled = cost.IndexOf("-") != -1;
                return;
            }
            if (e.Key != Key.Tab && cost.IndexOf(".") != -1)
            {
                int littleNumberCount = cost.Split('.')[1].Length;
                e.Handled = littleNumberCount >= 2;
                return;
            }
        }

        private bool IsNumber(Key key)
        {
            bool result = key == Key.D1 || key == Key.D2 || key == Key.D3
                || key == Key.D4 || key == Key.D5 || key == Key.D6
                || key == Key.D7 || key == Key.D8 || key == Key.D9 || key == Key.D0
                || key == Key.NumPad1 || key == Key.NumPad2 || key == Key.NumPad3
                || key == Key.NumPad4 || key == Key.NumPad5 || key == Key.NumPad6
                || key == Key.NumPad7 || key == Key.NumPad8 || key == Key.NumPad9 || key == Key.NumPad0
                || key == Key.Decimal || key == Key.OemPeriod || key == Key.Tab || key == Key.Subtract
                || key == Key.OemMinus;
            return result;
        }

    }
}

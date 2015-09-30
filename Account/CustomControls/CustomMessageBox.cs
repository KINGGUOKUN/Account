using System;
using System.Windows;
using System.Windows.Controls;

namespace GuoKun.CustomControls
{
    public class CustomMessageBox : Window
    {
        private string _message;
        private TextBlock _txtDisplay;
        private ContentControl _content;
        
        public CustomMessageBox(string message)
        {
            this._message = message;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("GuoKun.CustomControls;component/Themes/CustomMessageBox_Style.xaml", UriKind.Relative) }); 
            this._txtDisplay = new TextBlock();
            this._txtDisplay.Style = this.FindResource("CustomMessageBox_TextBlock") as Style;
            this.Style = this.FindResource("CustomMessageBox") as Style;
            this._txtDisplay.Text = this._message;
            this._content = new ContentControl();
            this._content.Content = this._txtDisplay;
            this.Content = this._content;
        }
    }
}

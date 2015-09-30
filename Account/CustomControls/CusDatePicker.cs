using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace GuoKun.CustomControls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CustomControls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CustomControls;assembly=CustomControls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class CusDatePicker : Control
    {
        public static readonly DependencyProperty DisplayModeProperty;
        public static readonly DependencyProperty SelectedTimeProperty;
        public static readonly DependencyProperty DisplayTimeStartProperty;
        public static readonly DependencyProperty DisplayTimeEndProperty;

        private TextBox _textbox;
        private Calendar _calendar;
        private Popup _popup;
        private Button _button;
        
        static CusDatePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CusDatePicker), new FrameworkPropertyMetadata(typeof(CusDatePicker)));
            DisplayModeProperty = DependencyProperty.Register("Displaymode", typeof(CalendarMode), typeof(CusDatePicker), new PropertyMetadata() { DefaultValue = CalendarMode.Year});
            SelectedTimeProperty = DependencyProperty.Register("SelectedTime", typeof(string), typeof(CusDatePicker));
            DisplayTimeStartProperty = DependencyProperty.Register("DisplayTimeStart", typeof(string), typeof(CusDatePicker),
                new PropertyMetadata(new PropertyChangedCallback(DisplayTimeStartPropertyChanged)));
            DisplayTimeEndProperty = DependencyProperty.Register("DisplayTimeEnd", typeof(string), typeof(CusDatePicker),
                new PropertyMetadata(new PropertyChangedCallback(DisplayTimeEndPropertyChanged)));
        }

        public CalendarMode DisplayMode
        {
            get
            {
                return (CalendarMode)this.GetValue(DisplayModeProperty);
            }
            set
            {
                this.SetValue(DisplayModeProperty, value);
            }
        }

        public string SelectedTime
        {
            get
            {
                return this.GetValue(SelectedTimeProperty) as string;
            }
            set
            {
                base.SetValue(SelectedTimeProperty, value);
            }
        }

        public string DisplayTimeStart
        {
            get
            {
                return base.GetValue(DisplayTimeStartProperty) as string;
            }
            set
            {
                DateTime start = DateTime.Now;
                if (DateTime.TryParse(value, out start))
                {
                    _calendar.DisplayDateStart = start;
                }
                base.SetValue(DisplayTimeStartProperty, value);
            }
        }

        public string DisplayTimeEnd
        {
            get
            {
                return base.GetValue(DisplayTimeEndProperty) as string;
            }
            set
            {
                DateTime end = DateTime.Now;
                if (DateTime.TryParse(value, out end))
                {
                    _calendar.DisplayDateEnd = end;
                }
                base.SetValue(DisplayTimeEndProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this._textbox = base.GetTemplateChild("PART_TextBox") as TextBox;
            this._popup = base.GetTemplateChild("PART_Popup") as Popup;
            this._calendar = base.GetTemplateChild("PART_Calendar") as Calendar;
            this._button = base.GetTemplateChild("PART_Button") as Button;
            this.Bind();
        }

        private void Bind()
        {
            this._calendar.Measure(new Size(180, 165));
            Image img = new Image();
            img.Width = this._button.Width;
            img.Height = this._button.Height;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("pack://application:,,,/GuoKun.CustomControls;component/resources/calendar.bmp");
            bitmap.EndInit();
            img.Source = bitmap;
            this._button.Content = img;
            this._calendar.DisplayMode = DisplayMode;
            DateTime start, end;
            if(this.DisplayMode == CalendarMode.Year)
            {
                if (DateTime.TryParse(DisplayTimeStart, out start))
                {
                    this._calendar.DisplayDateStart = start;
                }
                if (DateTime.TryParse(DisplayTimeEnd, out end))
                {
                    this._calendar.DisplayDateEnd = end;
                }
            }
            else if(this.DisplayMode == CalendarMode.Decade)
            {
                int year;
                if(int.TryParse(this.DisplayTimeStart, out year))
                {
                    start = new DateTime(year, 6, 8);
                    this._calendar.DisplayDateStart = start;
                }
                if (int.TryParse(this.DisplayTimeEnd, out year))
                {
                    end = new DateTime(year, 6, 8);
                    this._calendar.DisplayDateEnd = end;
                }
            }
            this._textbox.Text = this.GetValue(SelectedTimeProperty) as string;
            if (string.IsNullOrWhiteSpace(this._textbox.Text))
            {
                if (DisplayMode == CalendarMode.Year)
                {
                    this._textbox.Text = "选择月份";
                }
                else if (DisplayMode == CalendarMode.Decade)
                {
                    this._textbox.Text = "选择年份";
                }
            }
            this._button.Click += new RoutedEventHandler(_button_Click);
            this._calendar.DisplayModeChanged += new EventHandler<CalendarModeChangedEventArgs>(_calendar_DisplayModeChanged);
        }

        private void _calendar_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            if((int)this._calendar.DisplayMode + 1 == (int)DisplayMode)
            {
                this._popup.IsOpen = false;
                this.DisplaySelection();
            }
        }

        private void _button_Click(object sender, EventArgs e)
        {
            this._popup.IsOpen = true;
            this._calendar.DisplayMode = this.DisplayMode;
        }

        private void DisplaySelection()
        {
            if(DisplayMode == CalendarMode.Year)
            {
                this._textbox.Text = this._calendar.DisplayDate.ToString("yyyy-MM");
            }
            else if (DisplayMode == CalendarMode.Decade)
            {
                this._textbox.Text = this._calendar.DisplayDate.ToString("yyyy");
            }
            this.SetValue(SelectedTimeProperty, this._textbox.Text);
        }

        private static void DisplayTimeStartPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CusDatePicker picker = sender as CusDatePicker;
            Calendar calendar = picker._calendar;
            if(calendar == null)
            {
                return;
            }
            calendar.DisplayDateStart = null;
            if(e.NewValue == null)
            {
                return;
            }
            CalendarMode displaymode = picker.DisplayMode;
            DateTime start;
            if(displaymode == CalendarMode.Year)
            {
                if (DateTime.TryParse(e.NewValue.ToString(), out start))
                {
                    calendar.DisplayDateStart = start;
                }
                if (!string.IsNullOrWhiteSpace(picker.SelectedTime))
                {
                    if (DateTime.TryParse(picker.SelectedTime, out start))
                    {
                        calendar.DisplayDate = start;
                    }
                }
            }
            else if(displaymode == CalendarMode.Decade)
            {
                int year;
                if (int.TryParse(e.NewValue.ToString(), out year))
                {
                    start = new DateTime(year, 6, 8);
                    calendar.DisplayDateStart = start;
                }
                if (!string.IsNullOrWhiteSpace(picker.SelectedTime))
                {
                    if (int.TryParse(picker.SelectedTime, out year))
                    {
                        start = new DateTime(year, 6, 8);
                        calendar.DisplayDate = start;
                    }
                }
            }
        }

        private static void DisplayTimeEndPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CusDatePicker picker = sender as CusDatePicker;
            Calendar calendar = picker._calendar;
            if(calendar == null)
            {
                return;
            }
            calendar.DisplayDateEnd = null;
            if(e.NewValue == null)
            {
                return;
            }
            CalendarMode displaymode = picker.DisplayMode;
            DateTime end;
            if(displaymode == CalendarMode.Year)
            {
                if (DateTime.TryParse(e.NewValue.ToString(), out end))
                {
                    calendar.DisplayDateEnd = end;
                }
                if (!string.IsNullOrWhiteSpace(picker.SelectedTime))
                {
                    if(DateTime.TryParse(picker.SelectedTime, out end))
                    {
                        calendar.DisplayDate = end;
                    }
                }
            }
            else if(displaymode == CalendarMode.Decade)
            {
                int year;
                if(int.TryParse(e.NewValue.ToString(), out year))
                {
                    end = new DateTime(year, 6, 8);
                    calendar.DisplayDateEnd = end;
                }
                if(!string.IsNullOrWhiteSpace(picker.SelectedTime))
                {
                    if(int.TryParse(picker.SelectedTime, out year))
                    {
                        end = new DateTime(year, 6, 8);
                        calendar.DisplayDate = end;
                    }
                }
            } 
        }
    }
}

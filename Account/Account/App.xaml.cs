using GuoKun.Configuration;
using log4net;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.Windows;

namespace Account
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : SingletonApplication
    {
        private ILog _log;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //显示初始化提示框
            GuoKun.CustomControls.CustomMessageBox messageBox = new GuoKun.CustomControls.CustomMessageBox("系统正在初始化······");
            messageBox.Show();
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //加载系统样式
            ResourceDictionary defaultDictionary = new ResourceDictionary()
            {
                Source = new Uri("/GuoKun.Themes;component/Themes/Default.xaml", UriKind.RelativeOrAbsolute),
            };
            this.Resources.MergedDictionaries.Add(defaultDictionary);

            //系统初始化
            BootstrapperConfiguration bootstrapperConfiguration = ConfigurationManager.GetSection("account/bootstrapper") as BootstrapperConfiguration;
            CustomMefBootstrapper<Shell> bootstrapper = new CustomMefBootstrapper<Shell>(bootstrapperConfiguration);
            bootstrapper.Run(true);

            _log = UnityContainerFactory.GetUnityContainer().Resolve<ILog>();

            messageBox.Close();
            Application.Current.MainWindow.Show();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception excep = e.ExceptionObject as Exception;
            if (excep != null)
            {
                _log.Fatal(excep);
            }
            MessageBox.Show("系统发生未预料异常，将退出");
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            _log.Fatal(e.Exception);
            MessageBox.Show("系统发生未预料异常，将退出");
        }
    }
}

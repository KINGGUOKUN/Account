using log4net;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.Unity;
using Prism.Logging;
using Prism.Mef;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Threading;
using System.Windows;

namespace GuoKun.Configuration
{
    public class CustomMefBootstrapper<T> : 
        MefBootstrapper where T : DependencyObject
    {
        #region Private Fields

        private readonly BootstrapperConfiguration _configuration;

        private ILog _log;

        private List<AutoResetEvent> _autoResetEvents = new List<AutoResetEvent>();

        #endregion

        #region Constructors

        public CustomMefBootstrapper(BootstrapperConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration为空");
            }
            _configuration = configuration;
        }

        #endregion

        public override void Run(bool runWithDefaultConfiguration)
        {
            base.Run(runWithDefaultConfiguration);
            _autoResetEvents.ForEach(e => e.WaitOne());
        }

        protected override ILoggerFacade CreateLogger()
        {
            string logConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configuration.LogConfig);
            log4net.Config.XmlConfigurator.Configure(new FileStream(logConfigPath, FileMode.Open));
            _log = LogManager.GetLogger(_configuration.LogName);
            return base.CreateLogger();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance<ILog>(_log);
            UnityContainerFactory.RegisterUnityContainer(container);
        }

        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(T).Assembly));
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        protected override DependencyObject CreateShell()
        {
            //this.InitThems();
            this.InitializeDatabase();
            return this.Container.GetExportedValue<T>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        //private void InitThems()
        //{
        //    AsyncOperation operation = AsyncOperationManager.CreateOperation(null);
        //    AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        //    _autoResetEvents.Add(autoResetEvent);
        //    ThreadPool.QueueUserWorkItem(o =>
        //    {
        //        AsyncOperation asyncOperation = o as AsyncOperation;
        //        try
        //        {
        //            ResourceDictionary defaultDictionary = new ResourceDictionary()
        //            {
        //                Source = new Uri("/GuoKun.Themes;component/Themes/Default.xaml", UriKind.RelativeOrAbsolute),
        //            };
        //            _application.Resources.MergedDictionaries.Add(defaultDictionary);
        //        }
        //        catch(Exception e)
        //        {
        //            _log.Fatal("初始化系统样式异常:", e);
        //            asyncOperation.Post
        //                (
        //                    x =>
        //                    {
        //                        MessageBox.Show("初始化系统样式异常，系统将退出！");
        //                        Environment.Exit(-1);
        //                    },
        //                     null
        //                 );
        //        }
        //        asyncOperation.OperationCompleted();
        //        autoResetEvent.Set();
        //    }, operation);
        //}

        private void InitializeDatabase()
        {
            AsyncOperation operation = AsyncOperationManager.CreateOperation(null);
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            _autoResetEvents.Add(autoResetEvent);
            ThreadPool.QueueUserWorkItem(o =>
            {
                AsyncOperation asyncOperation = o as AsyncOperation;
                try
                {
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configuration.DbConfigFile);
                    IConfigurationSource configurationSource = new FileConfigurationSource(path);
                    DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(configurationSource));
                    DatabaseFactory.CreateDatabase().CreateConnection().Open();
                }
                catch (Exception e)
                {
                    _log.Fatal("初始化数据库异常:", e);
                    asyncOperation.Post
                        (
                            x =>
                            {
                                MessageBox.Show("初始化数据库异常，系统将退出！");
                                Environment.Exit(-1);
                            },
                             null
                         );
                }
                asyncOperation.OperationCompleted();
                autoResetEvent.Set();
            }, operation);         
        }
    }
}

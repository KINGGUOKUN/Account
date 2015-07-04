using Account.DailyManagement.Entities;
using Account.DailyManagement.ServiceImplement;
using Account.Entity.CompositeEvent;
using GuoKun.Configuration;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Account.DailyManagement.ViewModel
{
    [Export]
    public class VMDailyManagement : BindableBase
    {
        #region Private Fields

        private readonly IUnityContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDailyManager _dailyManagement;
        private SubscriptionToken _scptManifestRefresh;

        private DateTime _start;
        private DateTime _end;

        private TaskFactory<List<Daily>> _tf = new TaskFactory<List<Daily>>();
        private TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        private DailyModel _selectedItem;
        private ObservableCollection<DailyModel> _dailys;

        private bool _showNotFoundMessage;
        private Visibility _refreshingVisibility = Visibility.Collapsed;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public VMDailyManagement(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            DateTime now = DateTime.Now.Date;
            int subDays = (int)now.DayOfWeek;
            if (subDays == 0)
            {
                this._start = now.AddDays(-6);
            }
            else
            {
                this._start = now.AddDays(1 - subDays);
            }
            this._end = DateTime.Now;
            this._dailys = new ObservableCollection<DailyModel>();
            _container = UnityContainerFactory.GetUnityContainer();
            _dailyManagement = _container.Resolve<IDailyManager>();
            _scptManifestRefresh = _eventAggregator.GetEvent<ManifestRefreshEvent>().Subscribe(this._manifestRefresh, ThreadOption.UIThread);
            this.Refresh = new DelegateCommand<string>(RefreshExecute, CanExecuteRefresh);
            this.RefreshDaily(false);
        }

        #endregion

        #region Commands

        public ICommand Refresh { get; set; }

        #endregion

        #region Command Executes

        private void RefreshExecute(string param)
        {
            DateTime now = DateTime.Now.Date;
            if (param == "reset")
            {
                this.Start = now.AddDays(-now.Day + 1);
                this.End = this.Start.AddMonths(1).AddDays(-1);
            }
            else if (param == "refresh")
            {
                this.Start = new DateTime(now.Year, 1, 1);
                this.End = this.Start.AddYears(1).AddDays(-1);
            }
            else if (param == "search")
            {
            }
            this.RefreshDaily(true);
        }

        #endregion

        #region Can Execute

        private bool CanExecuteRefresh(string param)
        {
            return true;
        }

        #endregion

        #region Private Methods

        private void RefreshDaily(bool showNotFoundMessage)
        {
            this.RefreshingVisibility = Visibility.Visible;
            this._showNotFoundMessage = showNotFoundMessage;
            this._dailys.Clear();
            this._tf.StartNew(this.GetDailys).ContinueWith(this.GetDailysCompleted, this._scheduler);
        }

        private List<Daily> GetDailys()
        {
            List<Daily> result = _dailyManagement.GetDailys(this._start, this._end);
            return result;
        }

        private void GetDailysCompleted(Task<List<Daily>> task)
        {
            if (task.Exception != null)
            {
                this.RefreshingVisibility = Visibility.Collapsed;
                MessageBox.Show("获取日结算出错");
                return;
            }
            List<Daily> result = task.Result;
            if (result == null || result.Count == 0)
            {
                this.RefreshingVisibility = Visibility.Collapsed;
                if (this._showNotFoundMessage)
                {
                    MessageBox.Show("未查询到相关日结算信息！");
                }
                return;
            }
            DailyModel model;
            foreach (Daily d in result)
            {
                model = new DailyModel(d);
                this._dailys.Add(model);
            }
            this.RefreshingVisibility = Visibility.Collapsed;
        }

        private void _manifestRefresh(object obj)
        {
            this.RefreshDaily(false);
        }

        #endregion

        #region Public Properties

        public DateTime Start
        {
            get
            {
                return _start;
            }
            set
            {
                base.SetProperty(ref _start, value);
            }
        }

        public DateTime End
        {
            get
            {
                return _end;
            }
            set
            {
                base.SetProperty(ref _end ,value);
            }
        }

        public ObservableCollection<DailyModel> Dailys
        {
            get { return this._dailys; }
        }

        public DailyModel SelectedItem
        {
            get
            {
                return this._selectedItem;
            }
            set
            {
                base.SetProperty(ref _selectedItem, value);
            }
        }

        public Visibility RefreshingVisibility
        {
            get
            {
                return _refreshingVisibility;
            }
            private set
            {
                base.SetProperty(ref _refreshingVisibility, value);
                base.OnPropertyChanged("IsBtnEnable");
            }
        }

        public bool IsBtnEnable
        {
            get
            {
                return !(this.RefreshingVisibility == Visibility.Visible);
            }
        }

        #endregion
    }
}

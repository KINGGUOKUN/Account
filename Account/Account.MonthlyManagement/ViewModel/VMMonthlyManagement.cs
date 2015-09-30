using Account.Entity.CompositeEvent;
using Account.MonthlyManagement.Entities;
using Account.MonthlyManagement.ServiceImplement;
using GuoKun.Configuration;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Account.MonthlyManagement.ViewModel
{
    [Export]
    public class VMMonthlyManagement : BindableBase
    {
        #region Private Fields

        private string _start;
        private string _end;

        private TaskFactory<List<Monthly>> _tf = new TaskFactory<List<Monthly>>();
        private TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        private MonthlyModel _selectedItem;
        private ObservableCollection<MonthlyModel> _monthlys;

        private IMonthlyManager _monthlyManager;
        private IEventAggregator _eventAggregator;
        private SubscriptionToken _scptManifestRefresh;
        private bool _showNotFoundMessage;
        private Visibility _refreshingVisibility = Visibility.Collapsed;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public VMMonthlyManagement(IEventAggregator eventAggregator)
        {
            this._start = DateTime.Now.AddMonths(-3).ToString("yyyy-MM");
            this._end = DateTime.Now.Date.ToString("yyyy-MM");
            this._monthlys = new ObservableCollection<MonthlyModel>();
            this._monthlyManager = UnityContainerFactory.GetUnityContainer().Resolve<IMonthlyManager>();
            this._eventAggregator = eventAggregator;
            this._scptManifestRefresh = this._eventAggregator.GetEvent<ManifestRefreshEvent>().Subscribe(this._manifestRefresh, ThreadOption.UIThread);
            this.Refresh = new DelegateCommand<string>(RefreshExecute, CanExecuteRefresh);
            this.RefreshMonthly(false);
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
                this.Start = new DateTime(now.Year, 1, 1).ToString("yyyy-MM");
                this.End = now.ToString("yyyy-MM");
            }
            else if (param == "refresh")
            {
                this.Start = string.Empty;
                this.End = now.ToString("yyy-MM");
            }
            else if (param == "search")
            {
            }
            this.RefreshMonthly(true);
        }

        #endregion

        #region Can Execute

        private bool CanExecuteRefresh(string param)
        {
            return true;
        }

        #endregion

        #region Private Methods

        private void RefreshMonthly(bool showNotFoundMessage)
        {
            this.RefreshingVisibility = Visibility.Visible;
            this._showNotFoundMessage = showNotFoundMessage;
            this._monthlys.Clear();
            this._tf.StartNew(this.GetMonthlys).ContinueWith(this.GetMonthlysCompleted, this._scheduler);
        }

        private List<Monthly> GetMonthlys()
        {
            List<Monthly> result = this._monthlyManager.GetMonthlys(this._start, this._end);
            return result;
        }

        private void GetMonthlysCompleted(Task<List<Monthly>> task)
        {
            if (task.Exception != null)
            {
                this.RefreshingVisibility = Visibility.Collapsed;
                MessageBox.Show("获取月结算出错");
                return;
            }
            List<Monthly> result = task.Result;
            if (result == null || result.Count == 0)
            {
                if (this._showNotFoundMessage)
                {
                    MessageBox.Show("未查询到相关月结算信息！");
                }
                this.RefreshingVisibility = Visibility.Collapsed;
                return;
            }
            MonthlyModel model;
            foreach (Monthly m in result)
            {
                model = new MonthlyModel(m);
                this._monthlys.Add(model);
            }
            this.RefreshingVisibility = Visibility.Collapsed;
        }

        private void _manifestRefresh(object obj)
        {
            this.RefreshMonthly(false);
        }

        #endregion

        #region Public Properties

        public string Start
        {
            get
            {
                return this._start;
            }
            set
            {
                base.SetProperty(ref _start, value);
            }
        }

        public string End
        {
            get
            {
                return this._end;
            }
            set
            {
                base.SetProperty(ref _end, value);
            }
        }

        public ObservableCollection<MonthlyModel> Monthlys
        {
            get { return this._monthlys; }
        }

        public MonthlyModel SelectedItem
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

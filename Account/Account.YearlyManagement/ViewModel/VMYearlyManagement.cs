using Account.Entity.CompositeEvent;
using Account.YearlyManagement.Entities;
using Account.YearlyManagement.ServiceImplement;
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

namespace Account.YearlyManagement.ViewModel
{
    [Export]
    public class VMYearlyManagement : BindableBase
    {
        #region Private Fields

        private string _start;
        private string _end;

        private TaskFactory<List<Yearly>> _tf = new TaskFactory<List<Yearly>>();
        private TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        private YearlyModel _selectedItem;
        private ObservableCollection<YearlyModel> _yearlys;

        private IYearlyManager _yearlyManager;

        private IEventAggregator _eventAggregator;
        private SubscriptionToken _scptManifestRefresh;
        private bool _showNotFoundMessage;
        private Visibility _refreshingVisibility = Visibility.Collapsed;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public VMYearlyManagement(IEventAggregator eventAggregator)
        {
            this._start = DateTime.Now.AddYears(-3).Year.ToString();
            this._end = DateTime.Now.Year.ToString();
            this._yearlys = new ObservableCollection<YearlyModel>();
            this._yearlyManager = UnityContainerFactory.GetUnityContainer().Resolve<IYearlyManager>();
            this._eventAggregator = eventAggregator;
            this._scptManifestRefresh = this._eventAggregator.GetEvent<ManifestRefreshEvent>().Subscribe(this._manifestRefresh, ThreadOption.UIThread);
            this.Refresh = new DelegateCommand<string>(RefreshExecute, CanExecuteRefresh);
            this.RefreshYearly(false);
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
                this.Start = DateTime.Now.AddYears(-3).Year.ToString();
                this.End = DateTime.Now.Year.ToString();
            }
            else if (param == "refresh")
            {
                this.Start = string.Empty;
                this.End = DateTime.Now.Year.ToString();
            }
            else if (param == "search")
            {
            }
            this.RefreshYearly(true);
        }

        #endregion

        #region Can Execute

        private bool CanExecuteRefresh(string param)
        {
            return true;
        }

        #endregion

        #region Private Methods

        private void RefreshYearly(bool showNotFoundMessage)
        {
            this.RefreshingVisibility = Visibility.Visible;
            this._showNotFoundMessage = showNotFoundMessage;
            this._yearlys.Clear();
            this._tf.StartNew(this.GetYearlys).ContinueWith(this.GetYearlysCompleted, this._scheduler);
        }

        private List<Yearly> GetYearlys()
        {
            List<Yearly> result = this._yearlyManager.GetYearlys(this._start, this._end);
            return result;
        }

        private void GetYearlysCompleted(Task<List<Yearly>> task)
        {
            if (task.Exception != null)
            {
                this.RefreshingVisibility = Visibility.Collapsed;
                MessageBox.Show("获取年结算出错");
                return;
            }
            List<Yearly> result = task.Result;
            if (result == null || result.Count == 0)
            {
                if (this._showNotFoundMessage)
                {
                    MessageBox.Show("未查询到相关年结算信息！");
                }
                this.RefreshingVisibility = Visibility.Collapsed;
                return;
            }
            YearlyModel model;
            foreach (Yearly year in result)
            {
                model = new YearlyModel(year);
                this._yearlys.Add(model);
            }
            this.RefreshingVisibility = Visibility.Collapsed;
        }

        private void _manifestRefresh(object obj)
        {
            this.RefreshYearly(false);
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

        public ObservableCollection<YearlyModel> Yearlys
        {
            get { return this._yearlys; }
        }

        public YearlyModel SelectedItem
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

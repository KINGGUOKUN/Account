using Account.Entity;
using Account.Entity.CompositeEvent;
using Account.ManifestManagement.Entities;
using Account.ManifestManagement.ServiceImplement;
using Account.ManifestManagement.View;
using GuoKun.Configuration;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Account.ManifestManagement.ViewModel
{
    [Export]
    public class VMManifestManagement : BindableBase
    {
        #region Private Fields

        private readonly IUnityContainer _container;
        private readonly IManifestManager _manifestManager;

        private DateTime _start;
        private DateTime _end;
        private DateTime _generated;
        private ObservableCollection<ManifestModel> _manifests;

        private TaskFactory<List<Manifest>> _factory = new TaskFactory<List<Manifest>>();
        private TaskScheduler _schedule = TaskScheduler.FromCurrentSynchronizationContext();
        private ManifestModel _selectedItem;
        private readonly IEventAggregator _eventAggregator;
        private bool _showNotFoundMessage;
        private Visibility _refreshingVisibility = Visibility.Collapsed;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public VMManifestManagement(IEventAggregator eventAggregator)
        {
            _container = UnityContainerFactory.GetUnityContainer();
            _eventAggregator = eventAggregator;
            this._manifestManager = _container.Resolve<IManifestManager>();
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
            this._end = DateTime.Now.Date;
            this._manifests = new ObservableCollection<ManifestModel>();
            this.Add = new DelegateCommand(AddExecute, CanExecuteAdd);
            this.Update = new DelegateCommand(UpdateExecute, CanExecuteUpdate);
            this.Delete = new DelegateCommand(DeleteExecute, CanExecuteDelete);
            this.Refresh = new DelegateCommand<string>(RefreshExecute, CanExecuteRefresh);
            this.RefreshManifest(false);
        }

        #endregion

        #region Commands

        public ICommand Add { get; set; }
        public ICommand Update { get; set; }
        public ICommand Delete { get; set; }
        public ICommand Refresh { get; set; }

        #endregion

        #region Commands Executes

        private void AddExecute()
        {
            AddOrUpdateManifest winAdd = new AddOrUpdateManifest(null);
            winAdd.Owner = Application.Current.MainWindow;
            winAdd.HandleCompleted += new EventHandler<EntityEventArgs>(AddOrUpdate_HandleCompleted);
            winAdd.ShowDialog();
        }

        private void UpdateExecute()
        {
            AddOrUpdateManifest winUpdate = new AddOrUpdateManifest(this.SelectedItem);
            winUpdate.Owner = Application.Current.MainWindow;
            winUpdate.HandleCompleted += new EventHandler<EntityEventArgs>(AddOrUpdate_HandleCompleted);
            winUpdate.ShowDialog();
        }

        private void DeleteExecute()
        {
            bool result = false;
            MessageBoxResult msgBoxResult = MessageBox.Show("确认删除？", "删除", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgBoxResult == MessageBoxResult.No)
            {
                return;
            }
            result = this._manifestManager.DeleteManifest(this._selectedItem.ID);
            if (result)
            {
                this.Manifests.Remove(this.SelectedItem);
                this._eventAggregator.GetEvent<ManifestRefreshEvent>().Publish(this._selectedItem);
                MessageBox.Show("删除消费明细成功!");
            }
            else
            {
                MessageBox.Show("删除消费明细失败!");
            }
        }

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
            this.RefreshManifest(true);
        }

        #endregion

        #region Can Executes

        private bool CanExecuteAdd()
        {
            return true;
        }

        private bool CanExecuteUpdate()
        {
            return true;
        }

        private bool CanExecuteDelete()
        {
            return true;
        }

        private bool CanExecuteRefresh(string param)
        {
            return true;
        }

        #endregion

        #region Private Methods

        private void RefreshManifest(bool showNotFoundMessage)
        {
            this.RefreshingVisibility = Visibility.Visible;
            this._showNotFoundMessage = showNotFoundMessage;
            this._manifests.Clear();
            this._factory.StartNew(GetManifests).ContinueWith(GetManifestsCompleted, _schedule);
        }

        private List<Manifest> GetManifests()
        {
            List<Manifest> result = this._manifestManager.GetManifest(this._start, this._end);
            return result;
        }

        private void GetManifestsCompleted(Task<List<Manifest>> task)
        {
            if (task.Exception != null)
            {
                this.RefreshingVisibility = Visibility.Collapsed;
                MessageBox.Show("获取消费明细出错!");
                return;
            }
            List<Manifest> list = task.Result;
            if (list == null || list.Count == 0)
            {
                this.RefreshingVisibility = Visibility.Collapsed;
                if (this._showNotFoundMessage)
                {
                    MessageBox.Show("未查询到相关消费明细!");
                }
                return;
            }
            ManifestModel model;
            foreach (Manifest m in list)
            {
                model = new ManifestModel(m);
                this._manifests.Add(model);
            }
            this.RefreshingVisibility = Visibility.Collapsed;
        }

        private void AddOrUpdate_HandleCompleted(object sender, EntityEventArgs e)
        {
            bool isAdd = e.IsAdd;
            ManifestModel entity = e.Entity as ManifestModel;
            if (isAdd)
            {
                if (this.Manifests.Count == 0)
                {
                    this.Manifests.Add(entity);
                    this._eventAggregator.GetEvent<ManifestRefreshEvent>().Publish(this.SelectedItem);
                    return;
                }
                if (entity.Date < this.Manifests.FirstOrDefault().Date)
                {
                    this.Manifests.Insert(0, entity);
                }
                else if (entity.Date > this.Manifests.Last().Date)
                {
                    this.Manifests.Add(entity);
                }
                else
                {
                    int index = -1;
                    var items1 = this.Manifests.Where(x => x.Date == entity.Date);
                    ManifestModel item;
                    if (items1 != null)
                    {
                        item = items1.Where(x => x.Cost > entity.Cost).FirstOrDefault();
                        if (item == null)
                        {
                            item = items1.LastOrDefault();
                            index = this.Manifests.IndexOf(item);
                            index++;
                        }
                        else
                        {
                            index = this.Manifests.IndexOf(item);
                        }
                    }
                    else
                    {
                        item = this.Manifests.Where(x => x.Date > entity.Date).First();
                        index = this.Manifests.IndexOf(item);
                    }

                    this.Manifests.Insert(index, entity);
                }
            }
            else
            {
                ManifestModel item = this.Manifests.Where(x => x.ID == entity.ID).First();
                int index = this.Manifests.IndexOf(item);
                this.Manifests[index] = entity;
            }
            this.SelectedItem = entity;
            this._eventAggregator.GetEvent<ManifestRefreshEvent>().Publish(this.SelectedItem);
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
                return this._end;
            }
            set
            {
                base.SetProperty(ref _end, value);
            }
        }

        public DateTime Generated
        {
            get
            {
                return _generated;
            }
            set
            {
                base.SetProperty(ref _generated, value);
            }
        }

        public ObservableCollection<ManifestModel> Manifests
        {
            get { return this._manifests; }
        }

        public ManifestModel SelectedItem
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

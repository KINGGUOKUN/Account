using Account.Entity;
using Account.ManifestManagement.ServiceImplement;
using GuoKun.Configuration;
using Microsoft.Practices.Unity;
using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace Account.ManifestManagement.ViewModel
{
    public class VMAddOrUpdateManifest : BaseViewModel
    {
        #region Private Fields

        private ManifestModel _model;
        private IManifestManager _manifestManager;
        private bool _isAdd;

        #endregion

        #region Constructor

        public VMAddOrUpdateManifest(ManifestModel model)
        {
            this._isAdd = model == null;
            if (_isAdd)
            {
                _model = new ManifestModel()
                {
                    Date = DateTime.Now.Date
                };
            }
            else
            {
                _model = new ManifestModel()
                {
                    ID = model.ID,
                    Date = model.Date,
                    Cost = model.Cost,
                    Remark = model.Remark
                };
            }
            this.Save = new DelegateCommand(ExecuteSave);
            this._manifestManager = UnityContainerFactory.GetUnityContainer().Resolve<IManifestManager>();
        }

        #endregion

        #region Commands

        /// <summary>
        /// 保存命令
        /// </summary>
        public ICommand Save
        {
            get;
            private set;
        }

        #endregion

        #region Command Executes


        private void ExecuteSave()
        {
            if (this._isAdd)
            {
                if (this._manifestManager.AddManifest(this._model.ToManifest()))
                {
                    if (this.HandleCompleted != null)
                    {
                        this.HandleCompleted(this, new EntityEventArgs(this._model, true));
                    }
                    MessageBox.Show("添加消费明细成功!");
                }
                else
                {
                    MessageBox.Show("添加消费明细失败!");
                }
            }
            else
            {
                if (this._manifestManager.UpdateManifest(this._model.ToManifest()))
                {
                    if (this.HandleCompleted != null)
                    {
                        this.HandleCompleted(this, new EntityEventArgs(this._model, false));
                    }
                    MessageBox.Show("更新消费明细成功!");
                }
                else
                {
                    MessageBox.Show("更新消费明细失败!");
                }
            }
        }

        #endregion

        #region Public Properties

        public bool IsComplete
        {
            get
            {
                return this.Date.HasValue
                    && !string.IsNullOrWhiteSpace(this.Cost)
                    && !string.IsNullOrWhiteSpace(this.Remark);
            }
        }

        public override bool CanSubmit
        {
            get
            {
                return this.IsComplete
                    && base.HasChanges
                    && !base.HasErrors;
            }
        }

        public DateTime? Date
        {
            get
            {
                return _model.Date;
            }
            set
            {
                if (!value.HasValue)
                {
                    base.OnPropertyChanged("Date");
                    return;
                }
                base.HasChanges = true;
                _model.Date = value.Value.Date;
                base.OnPropertyChanged("Date");
                base.RemoveError("Date");
            }
        }

        /// <summary>
        /// 消费金额
        /// </summary>
        public string Cost
        {
            get
            {
                return _model.Cost == 0 ? null : _model.Cost.ToString("F2");
            }
            set
            {
                string oldValue = _model.Cost == 0 ? null : _model.Cost.ToString();
                if (oldValue != value)
                {
                    base.HasChanges = true;
                }
                if (string.IsNullOrWhiteSpace(value))
                {
                    _model.Cost = 0;
                    base.OnPropertyChanged("Cost");
                    base.AddError("Cost", "消费金额不能为空");
                    return;
                }
                decimal temp = 0;
                if (!decimal.TryParse(value.Trim(), out temp))
                {
                    _model.Cost = 0;
                    base.OnPropertyChanged("Cost");
                    base.AddError("Cost", "消费金额输入非法");
                    return;
                }
                if (temp == 0)
                {
                    _model.Cost = 0;
                    base.OnPropertyChanged("Cost");
                    base.AddError("Cost", "消费金额不能为0");
                    return;
                }
                _model.Cost = temp;
                base.OnPropertyChanged("Cost");
                base.RemoveError("Cost");
            }
        }

        /// <summary>
        /// 消费明细
        /// </summary>
        public string Remark
        {
            get
            {
                return _model.Remark;
            }
            set
            {
                if (value != _model.Remark)
                {
                    base.HasChanges = true;
                }
                _model.Remark = value;
                if (string.IsNullOrWhiteSpace(value))
                {
                    base.AddError("Remark", "消费明细不能为空");
                    return;
                }
                base.OnPropertyChanged("Remark");
                base.RemoveError("Remark");
            }
        }

        #endregion

        #region Events

        public event EventHandler<EntityEventArgs> HandleCompleted;

        #endregion
    }
}

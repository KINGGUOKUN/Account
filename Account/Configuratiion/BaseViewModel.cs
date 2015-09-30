using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GuoKun.Configuration
{
    /// <summary>
    /// ViewModel抽象基类，增加了变化通知、数据验证功能
    /// </summary>  
    public abstract class BaseViewModel : BindableBase, IDataErrorInfo
    {
        #region Private Fields

        /// <summary>
        /// 错误集合
        /// </summary>
        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();

        /// <summary>
        /// 是否有变化
        /// </summary>
        private bool _hasChanges = false;

        #endregion

        #region Public Properties/Events

        /// <summary>
        /// 验证错误变化事件
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        ///  是否有验证错误
        /// </summary>
        public bool HasErrors
        {
            get
            {
                return _errors.Count > 0;
            }
        }

        /// <summary>
        /// 是否有绑定属性变化
        /// </summary>
        public bool HasChanges
        {
            get
            {
                return _hasChanges;
            }
            protected set
            {
                _hasChanges = value;
            }
        }

        /// <summary>
        /// 是否可提交
        /// </summary>
        public abstract bool CanSubmit
        {
            get;
        }

        /// <summary>
        /// IDataErrorInfo成员，一般不用
        /// </summary>
        public string Error
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// IDataErrorInfo成员，索引验证错误内容
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                string error = null;
                _errors.TryGetValue(columnName, out error);
                return error;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// 给指定属性添加指定验证错误
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="error"></param>
        protected void AddError(string propertyName, string error)
        {
            _errors[propertyName] = error;
            this.OnErrorsChanged(propertyName);
            base.OnPropertyChanged("CanSubmit");
        }

        /// <summary>
        /// 移除指定属性的验证错误
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RemoveError(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
                this.OnErrorsChanged(propertyName);
            }
            base.OnPropertyChanged("CanSubmit");
        }

        #endregion

        #region Private Method

        /// <summary>
        /// 引发验证错误变化事件
        /// </summary>
        /// <param name="propertyName"></param>
        private void OnErrorsChanged(string propertyName)
        {
            var handler = this.ErrorsChanged;
            if (handler != null)
            {
                handler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    /// <summary>
    /// 验证错误变化事件参数
    /// </summary>
    public sealed class DataErrorsChangedEventArgs : EventArgs
    {
        public DataErrorsChangedEventArgs(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        public string PropertyName
        {
            get;
            private set;
        }
    }
}

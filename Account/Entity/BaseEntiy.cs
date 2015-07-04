using System;
using System.ComponentModel;

namespace Account.Entity
{
    public class BaseEntiy : INotifyPropertyChanged
    {
        #region private field

        private Guid id;
        private decimal cost;
        private string remark;

        #endregion

        #region event

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        public BaseEntiy()
        {
            this.id = Guid.NewGuid();
        }

        #endregion

        #region Property

        public Guid ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public decimal Cost
        {
            get { return this.cost; }
            set
            {
                this.cost = value;
                OnPropetyChanged("Cost");
            }
        }

        public string Remark
        {
            get { return this.remark; }
            set
            {
                this.remark = value;
                OnPropetyChanged("Remark");
            }
        }

        #endregion

        #region public method



        #endregion

        #region protected method

        protected void OnPropetyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion


    }
}

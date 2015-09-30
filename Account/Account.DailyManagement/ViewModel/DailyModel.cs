using Account.DailyManagement.Entities;
using Account.Entity;
using System;

namespace Account.DailyManagement.ViewModel
{
    /// <summary>
    /// 日消费清单绑定实体
    /// </summary>
    public class DailyModel : BaseEntiy
    {
        private DateTime date;

        public DailyModel()
        {
        }

        public DailyModel(Daily daily)
        {
            this.ID = daily.ID;
            this.Date = daily.Date;
            this.Cost = daily.Cost;
            this.Remark = daily.Remark;
        }

        public DateTime Date
        {
            get { return this.date; }
            set
            {
                this.date = value;
                OnPropetyChanged("Date");
            }
        }

        public Daily ToDaily()
        {
            Daily daily = new Daily()
            {
                ID = this.ID,
                Date = this.Date,
                Cost = this.Cost,
                Remark = this.Remark
            };

            return daily;
        }
    }
}

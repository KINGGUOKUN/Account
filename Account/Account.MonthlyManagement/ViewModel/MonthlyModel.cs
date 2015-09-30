using Account.Entity;
using Account.MonthlyManagement.Entities;

namespace Account.MonthlyManagement.ViewModel
{
    public class MonthlyModel : BaseEntiy
    {
        private string month;

        public MonthlyModel()
        {
        }

        public MonthlyModel(Monthly monthly)
        {
            this.ID = monthly.ID;
            this.Month = monthly.Month;
            this.Cost = monthly.Cost;
            this.Remark = monthly.Remark;
        }

        public string Month
        {
            get { return this.month; }
            set
            {
                this.month = value;
                OnPropetyChanged("Month");
            }
        }

        public Monthly ToMonthly()
        {
            Monthly monthly = new Monthly()
            {
                ID = this.ID,
                Month = this.Month,
                Cost = this.Cost,
                Remark = this.Remark
            };

            return monthly;
        }
    }
}

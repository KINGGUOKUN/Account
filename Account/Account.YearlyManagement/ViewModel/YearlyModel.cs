using Account.Entity;
using Account.YearlyManagement.Entities;

namespace Account.YearlyManagement.ViewModel
{
    public class YearlyModel : BaseEntiy
    {
        private string year;

        public YearlyModel()
        {
        }

        public YearlyModel(Yearly yearly)
        {
            this.ID = yearly.ID;
            this.year = yearly.Year;
            this.Cost = yearly.Cost;
            this.Remark = yearly.Remark;
        }

        public string Year
        {
            get { return this.year; }
            set
            {
                this.year = value;
                OnPropetyChanged("Year");
            }
        }

        public Yearly ToYearly()
        {
            Yearly yearly = new Yearly()
            {
                ID = this.ID,
                Year = this.Year,
                Cost = this.Cost,
                Remark = this.Remark
            };

            return yearly;
        }
    }
}

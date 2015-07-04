using System;

namespace Account.MonthlyManagement.Entities
{
    public class Monthly
    {
        public Monthly()
        {
        }

        public Guid ID
        {
            get;
            set;
        }

        public string Month
        {
            get;
            set;
        }

        public decimal Cost
        {
            get;
            set;
        }

        public string Remark
        {
            get;
            set;
        }
    }
}

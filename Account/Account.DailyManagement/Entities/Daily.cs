using System;

namespace Account.DailyManagement.Entities
{
    public class Daily
    {
        public Daily()
        {
        }

        public Guid ID
        {
            get;
            set;
        }

        public DateTime Date
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

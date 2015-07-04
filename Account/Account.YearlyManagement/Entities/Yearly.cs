using System;

namespace Account.YearlyManagement.Entities
{
    public class Yearly
    {
        public Yearly()
        {
        }

        public Guid ID
        {
            get;
            set;
        }

        public string Year
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

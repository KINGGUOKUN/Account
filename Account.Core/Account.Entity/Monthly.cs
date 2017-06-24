using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Account.Entity
{
    public class Monthly
    {
        public string ID
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
    }
}

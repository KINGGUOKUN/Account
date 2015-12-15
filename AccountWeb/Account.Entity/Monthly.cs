using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Entity
{
    public class Monthly
    {
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
    }
}

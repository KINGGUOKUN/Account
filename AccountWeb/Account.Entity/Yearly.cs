using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Entity
{
    public class Yearly
    {
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
    }
}

using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Common
{
    public class Log
    {
        public static readonly ILog Logger = LogManager.GetLogger("Account");
    }
}

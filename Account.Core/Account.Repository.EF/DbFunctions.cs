using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Repository.EF
{
    public static class DbFunctions
    {
        [DbFunction("ExtractMonth")]
        public static string ExtractMonth(DateTime dateTime)
        {
            throw new Exception();
        }
    }
}

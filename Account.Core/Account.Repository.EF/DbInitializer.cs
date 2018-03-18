using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Account.Entity;

namespace Account.Repository.EF
{
    public static class DbInitializer
    {
        public static void Initialize(AccountContext context)
        {
            context.Database.EnsureCreated();

            if(context.Set<Manifest>().Any())
            {
                return;
            }
        }
    }
}

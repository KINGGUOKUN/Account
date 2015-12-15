using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Entity
{
    public class Daily
    {
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
    }

    public class DailyMapper : ClassMapper<Daily>
    {
        public DailyMapper()
        {
            Table("DAILY");
            Map(x => x.ID).Key(KeyType.Assigned);
            AutoMap();
        }
    }
}

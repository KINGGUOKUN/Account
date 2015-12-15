using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Entity
{
    public class Manifest
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

        public string Remark
        {
            get;
            set;
        }
    }

    public class ManifestMapper : ClassMapper<Manifest>
    {
        public ManifestMapper()
        {
            Table("MANIFEST");
            Map(x => x.ID).Key(KeyType.Assigned);
            AutoMap();
        }
    }
}

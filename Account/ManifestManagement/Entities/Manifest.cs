using System;

namespace Account.ManifestManagement.Entities
{
    public class Manifest
    {
        public Manifest()
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

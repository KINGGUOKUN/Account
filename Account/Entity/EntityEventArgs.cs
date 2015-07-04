using System;

namespace Account.Entity
{
    public class EntityEventArgs : EventArgs
    {
        public EntityEventArgs(object entity, bool isAdd)
        {
            this.Entity = entity;
            this.IsAdd = isAdd;
        }

        public object Entity { get; set; }
        public bool IsAdd { get; set; }
    }
}

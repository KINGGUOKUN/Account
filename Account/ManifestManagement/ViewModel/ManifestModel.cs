using Account.Entity;
using Account.ManifestManagement.Entities;
using System;

namespace Account.ManifestManagement.ViewModel
{
    public class ManifestModel : BaseEntiy
    {
        #region private field

        private DateTime date;

        #endregion

        #region Constructor

        public ManifestModel()
        {
        }

        public ManifestModel(Manifest manifest)
        {
            this.ID = manifest.ID;
            this.Date = manifest.Date;
            this.Cost = manifest.Cost;
            this.Remark = manifest.Remark;
        }

        #endregion

        #region Property

        public DateTime Date
        {
            get { return this.date; }
            set
            {
                this.date = value;
                this.OnPropetyChanged("Date");
            }
        }

        #endregion

        #region Public Method

        public Manifest ToManifest()
        {
            Manifest manifest = new Manifest()
            {
                ID = this.ID,
                Date = this.Date,
                Cost = this.Cost,
                Remark = this.Remark
            };

            return manifest;
        }

        #endregion
    }
}

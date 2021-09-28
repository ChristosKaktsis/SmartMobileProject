using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class ΤρόποςΠληρωμής : XPObject
    {
        public ΤρόποςΠληρωμής() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΤρόποςΠληρωμής(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        public Guid SmartOid
        {
            get { return smartOid; }
            set { SetPropertyValue(nameof(SmartOid), ref smartOid, value); }
        }
        Guid smartOid;

        public string Τρόποςπληρωμής
        {
            get { return τρόποςπληρωμής; }
            set { SetPropertyValue(nameof(Τρόποςπληρωμής), ref τρόποςπληρωμής, value); }
        }
        string τρόποςπληρωμής;

    }

}
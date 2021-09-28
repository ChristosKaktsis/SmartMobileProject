using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class ΤρόποςΑποστολής : XPObject
    {
        public ΤρόποςΑποστολής() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΤρόποςΑποστολής(Session session) : base(session)
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

        public string Τρόποςαποστολής
        {
            get { return τρόποςαποστολής; }
            set { SetPropertyValue(nameof(Τρόποςαποστολής), ref τρόποςαποστολής, value); }
        }
        string τρόποςαποστολής;
    }

}
using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class Χώρα : XPObject
    {
        public Χώρα() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Χώρα(Session session) : base(session)
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

        public string Συντομογραφία
        {
            get { return συντομογραφία; }
            set { SetPropertyValue(nameof(Συντομογραφία), ref συντομογραφία, value); }
        }
        string συντομογραφία;

        public string ΌνομαΧώρας
        {
            get { return χώρα; }
            set { SetPropertyValue(nameof(ΌνομαΧώρας), ref χώρα, value); }
        }
        string χώρα;
    }

}
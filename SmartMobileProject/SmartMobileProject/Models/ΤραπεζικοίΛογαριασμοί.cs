using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class ΤραπεζικοίΛογαριασμοί : XPObject
    {
        public ΤραπεζικοίΛογαριασμοί() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΤραπεζικοίΛογαριασμοί(Session session) : base(session)
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
        public string IBAN
        {
            get { return iban; }
            set { SetPropertyValue(nameof(IBAN), ref iban, value); }
        }
        string iban;
        public string Swift
        {
            get { return swift; }
            set { SetPropertyValue(nameof(Swift), ref swift, value); }
        }
        string swift;
        public string Λογαριασμός
        {
            get { return λογαριασμός; }
            set { SetPropertyValue(nameof(Λογαριασμός), ref λογαριασμός, value); }
        }
        string λογαριασμός;
        public Τράπεζα Τράπεζα
        {
            get { return τράπεζα; }
            set { SetPropertyValue(nameof(Τράπεζα), ref τράπεζα, value); }
        }
        Τράπεζα τράπεζα;

        public string Υποκατάστημα
        {
            get { return υποκατάστημα; }
            set { SetPropertyValue(nameof(Υποκατάστημα), ref υποκατάστημα, value); }
        }
        string υποκατάστημα;
    }

}
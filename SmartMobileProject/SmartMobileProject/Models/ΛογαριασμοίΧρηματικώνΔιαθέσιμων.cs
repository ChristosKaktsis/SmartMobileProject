using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class ΛογαριασμοίΧρηματικώνΔιαθέσιμων : XPObject
    {
        public ΛογαριασμοίΧρηματικώνΔιαθέσιμων() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΛογαριασμοίΧρηματικώνΔιαθέσιμων(Session session) : base(session)
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
        public ΤραπεζικοίΛογαριασμοί ΤραπεζικόςΛογαριασμός
        {
            get { return τραπεζικόςΛογαριασμός; }
            set { SetPropertyValue(nameof(ΤραπεζικόςΛογαριασμός), ref τραπεζικόςΛογαριασμός, value); }
        }
        ΤραπεζικοίΛογαριασμοί τραπεζικόςΛογαριασμός;
    }

}
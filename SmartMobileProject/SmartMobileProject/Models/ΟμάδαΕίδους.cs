using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class ΟμάδαΕίδους : XPObject
    {
        public ΟμάδαΕίδους() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΟμάδαΕίδους(Session session) : base(session)
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

        public string Ομάδα
        {
            get { return ομάδα; }
            set { SetPropertyValue(nameof(Ομάδα), ref ομάδα, value); }
        }
        string ομάδα;

        public Int32 ΣειράΕμφάνισης
        {
            get { return σειράεμφάνισης; }
            set { SetPropertyValue(nameof(ΣειράΕμφάνισης), ref σειράεμφάνισης, value); }
        }
        Int32 σειράεμφάνισης;

        public Single FontΜέγεθος
        {
            get { return fontμέγεθος; }
            set { SetPropertyValue(nameof(FontΜέγεθος), ref fontμέγεθος, value); }
        }
        Single fontμέγεθος;




    }

}
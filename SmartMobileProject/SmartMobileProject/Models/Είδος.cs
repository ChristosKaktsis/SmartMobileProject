using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class Είδος : XPObject
    {
        public Είδος() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Είδος(Session session) : base(session)
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

        public string Κωδικός
        {
            get { return κωδικός; }
            set { SetPropertyValue(nameof(Κωδικός), ref κωδικός, value); }
        }
        string κωδικός;

        public string Περιγραφή
        {
            get { return περιγραφή; }
            set { SetPropertyValue(nameof(Περιγραφή), ref περιγραφή, value); }
        }
        string περιγραφή;

        public ΦΠΑ ΦΠΑ
        {
            get { return φπα; }
            set { SetPropertyValue(nameof(ΦΠΑ), ref φπα, value); }
        }
        ΦΠΑ φπα;

        //start
        //Ομαδοποιησεις
        //
        public ΟικογένειαΕίδους Οικογένεια
        {
            get { return οικογένεια; }
            set { SetPropertyValue(nameof(Οικογένεια), ref οικογένεια, value); }
        }
        ΟικογένειαΕίδους οικογένεια;

        public ΥποοικογένειαΕίδους Υποοικογένεια
        {
            get { return υποοικογένεια; }
            set { SetPropertyValue(nameof(Υποοικογένεια), ref υποοικογένεια, value); }
        }
        ΥποοικογένειαΕίδους υποοικογένεια;


        public ΟμάδαΕίδους Ομάδα
        {
            get { return ομάδα; }
            set { SetPropertyValue(nameof(Ομάδα), ref ομάδα, value); }
        }
        ΟμάδαΕίδους ομάδα;

        public ΚατηγορίαΕίδους Κατηγορία
        {
            get { return κατηγορία; }
            set { SetPropertyValue(nameof(Κατηγορία), ref κατηγορία, value); }
        }
        ΚατηγορίαΕίδους κατηγορία;

        //end
        //ομαδοποιησεις
        //

        public double ΤιμήΧονδρικής
        {
            get { return τιμήχονδρικής; }
            set { SetPropertyValue(nameof(ΤιμήΧονδρικής), ref τιμήχονδρικής, value); }
        }
        double τιμήχονδρικής;
        public DateTime ΗμνίαΔημ
        {
            get { return ημνίαδημ; }
            set { SetPropertyValue(nameof(ΗμνίαΔημ), ref ημνίαδημ, value); }
        }
        DateTime ημνίαδημ;
        //
        [NonPersistent]
        Single ποσότητα;
        public Single Ποσότητα
        {
            get
            {
                return ποσότητα;
            }
            set { SetPropertyValue(nameof(Ποσότητα), ref ποσότητα, value); }
        }

    }

}
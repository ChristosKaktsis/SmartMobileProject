using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class ΣτοιχείαΕταιρίας : XPObject
    {
        public ΣτοιχείαΕταιρίας() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΣτοιχείαΕταιρίας(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }
       
        public string Επωνυμία
        {
            get { return επωνυμία; }
            set { SetPropertyValue(nameof(Επωνυμία), ref επωνυμία, value); }
        }
        string επωνυμία;
        
        public int ΚατηγορίαΦΠΑ
        {
            get { return κατηγορίαφπα; }
            set { SetPropertyValue(nameof(ΚατηγορίαΦΠΑ), ref κατηγορίαφπα, value); }
        }
        int κατηγορίαφπα;

        public string ΑΦΜ
        {
            get { return αφμ; }
            set { SetPropertyValue(nameof(ΑΦΜ), ref αφμ, value); }
        }
        string αφμ;
        public ΔΟΥ ΔΟΥ
        {
            get { return δου; }
            set { SetPropertyValue(nameof(ΔΟΥ), ref δου, value); }
        }
        ΔΟΥ δου;
        public string ΔικτυακόςΤόπος
        {
            get { return δικτυακόςτόπος; }
            set { SetPropertyValue(nameof(ΔικτυακόςΤόπος), ref δικτυακόςτόπος, value); }
        }
        string δικτυακόςτόπος;
        public string Οδός
        {
            get { return οδός; }
            set { SetPropertyValue(nameof(Οδός), ref οδός, value); }
        }
        string οδός;

        public string Αριθμός
        {
            get { return αριθμός; }
            set { SetPropertyValue(nameof(Αριθμός), ref αριθμός, value); }
        }
        string αριθμός;

        public string Περιοχή
        {
            get { return περιοχή; }
            set { SetPropertyValue(nameof(Περιοχή), ref περιοχή, value); }
        }
        string περιοχή;

        public string Τηλέφωνο
        {
            get { return τηλέφωνο; }
            set { SetPropertyValue(nameof(Τηλέφωνο), ref τηλέφωνο, value); }
        }
        string τηλέφωνο;

        public string Τηλέφωνο1
        {
            get { return τηλέφωνο1; }
            set { SetPropertyValue(nameof(Τηλέφωνο1), ref τηλέφωνο1, value); }
        }
        string τηλέφωνο1;
        public ΤαχυδρομικόςΚωδικός ΤΚ
        {
            get { return τκ; }
            set { SetPropertyValue(nameof(ΤΚ), ref τκ, value); }
        }
        ΤαχυδρομικόςΚωδικός τκ;
        public Πόλη Πόλη
        {
            get { return πόλη; }
            set { SetPropertyValue(nameof(Πόλη), ref πόλη, value); }
        }
        Πόλη πόλη;
        public string FAX
        {
            get { return fax; }
            set { SetPropertyValue(nameof(FAX), ref fax, value); }
        }
        string fax;
        public string Email
        {
            get { return email; }
            set { SetPropertyValue(nameof(Email), ref email, value); }
        }
        string email;
        public string UsernameΥπηρεσίαςΣτοιχείωνΜητρώου
        {
            get { return usernameΥπηρεσίαςΣτοιχείωνΜητρώου; }
            set { SetPropertyValue(nameof(UsernameΥπηρεσίαςΣτοιχείωνΜητρώου), ref usernameΥπηρεσίαςΣτοιχείωνΜητρώου, value); }
        }
        string usernameΥπηρεσίαςΣτοιχείωνΜητρώου;
        public string PasswordΥπηρεσίαςΣτοιχείωνΜητρώου
        {
            get { return passwordΥπηρεσίαςΣτοιχείωνΜητρώου; }
            set { SetPropertyValue(nameof(PasswordΥπηρεσίαςΣτοιχείωνΜητρώου), ref passwordΥπηρεσίαςΣτοιχείωνΜητρώου, value); }
        }
        string passwordΥπηρεσίαςΣτοιχείωνΜητρώου;
    }

}
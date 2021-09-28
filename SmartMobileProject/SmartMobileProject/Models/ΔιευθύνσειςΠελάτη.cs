using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class ΔιευθύνσειςΠελάτη : XPObject
    {
        public ΔιευθύνσειςΠελάτη() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΔιευθύνσειςΠελάτη(Session session) : base(session)
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
        
        public string Kίντηλέφωνο
        {
            get { return κίντηλέφωνο; }
            set { SetPropertyValue(nameof(Kίντηλέφωνο), ref κίντηλέφωνο, value); }
        }
        string κίντηλέφωνο;
        
        public string FAX
        {
            get { return fax; }
            set { SetPropertyValue(nameof(FAX), ref fax, value); }
        }
        string fax;

        public double ΓεωγραφικόΜήκος
        {
            get { return γεωγραφικόμήκος; }
            set { SetPropertyValue(nameof(ΓεωγραφικόΜήκος), ref γεωγραφικόμήκος, value); }
        }
        double γεωγραφικόμήκος;

        public double ΓεωγραφικόΠλάτος
        {
            get { return γεωγραφικόπλάτος; }
            set { SetPropertyValue(nameof(ΓεωγραφικόΠλάτος), ref γεωγραφικόπλάτος, value); }
        }
        double γεωγραφικόπλάτος;
        public bool CanUpload
        {
            get { return canUpload; }
            set { SetPropertyValue(nameof(CanUpload), ref canUpload, value); }
        }
        bool canUpload;
        public DateTime ΗμνίαΔημ
        {
            get { return ημνίαδημ; }
            set { SetPropertyValue(nameof(ΗμνίαΔημ), ref ημνίαδημ, value); }
        }
        DateTime ημνίαδημ;
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
        
        [Association]
        public Πελάτης Πελάτης
        {
            get { return πελάτης; }
            set { SetPropertyValue(nameof(Πελάτης), ref πελάτης, value); }
        }
        Πελάτης πελάτης;

        [NonPersistent]
        public string Addresstring
        {
            get
            {
                var returnString = "";
                if (this.Οδός == null)
                {
                    return returnString;
                }
                returnString += this.Οδός +" "+ this.Αριθμός+" ";
                if (this.ΤΚ != null)
                {
                    returnString += this.ΤΚ.Ονοματκ+" ";
                }
                if (this.Πόλη != null)
                {
                    returnString += this.Πόλη.ΟνομαΠόλης+" ";
                }
                returnString += this.Περιοχή;
              

                return returnString;
            }
        }
        [NonPersistent]
        string tk
        {
            get
            {
                if (ΤΚ != null)
                    return ΤΚ.Ονοματκ;
                return string.Empty;
            }
        }
        [NonPersistent]
        string poli
        {
            get
            {
                if (Πόλη != null)
                    return Πόλη.ΟνομαΠόλης.ToString();
                return string.Empty;
            }
        }
        [NonPersistent]
        string pelatis
        {
            get
            {
                if (Πελάτης != null)
                    return Πελάτης.SmartOid.ToString();
                return string.Empty;
            }
        }
        public string ToJson()
        {
            string json = string.Empty;
            json = "{ \"Oid\":\"" + smartOid +
                "\",\"Οδός\":\"" + (string.IsNullOrEmpty(Οδός) ? "" : Οδός) +
                "\",\"Αριθμός\":\"" + (string.IsNullOrEmpty(Αριθμός) ? "" : Αριθμός) +
                "\",\"Περιοχή\":\"" + (string.IsNullOrEmpty(Περιοχή) ? "" : Περιοχή) +
                "\",\"Τηλέφωνο\":\"" + (string.IsNullOrEmpty(Τηλέφωνο) ? "" : Τηλέφωνο) +
                "\",\"Τηλέφωνο1\":\"" + (string.IsNullOrEmpty(Τηλέφωνο1) ? "" : Τηλέφωνο1) +
                "\",\"KίνΤηλέφωνο\":\"" + (string.IsNullOrEmpty(Kίντηλέφωνο) ? "" : Kίντηλέφωνο) +
                "\",\"FAX\":\"" + (string.IsNullOrEmpty(FAX) ? "" : FAX) +
                "\",\"ΓεωγραφικόΜήκος\":\"" + (string.IsNullOrEmpty(ΓεωγραφικόΜήκος.ToString()) ? "" : ΓεωγραφικόΜήκος.ToString()) +
                "\",\"ΓεωγραφικόΠλάτος\":\"" + (string.IsNullOrEmpty(ΓεωγραφικόΠλάτος.ToString()) ? "" : ΓεωγραφικόΠλάτος.ToString()) +
                "\",\"ΤΚ\":\"" + tk +
                "\",\"Πόλη\":\"" + poli +
                "\",\"Πελάτης\":\"" + pelatis +
                "\"}";
            return json;
        }
    }

}
using DevExpress.Xpo;
using System;
using System.Linq;

namespace SmartMobileProject.Models
{
    public class Πελάτης : XPObject
    {
        public Πελάτης() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Πελάτης(Session session) : base(session)
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
        public Guid SmartOidΚεντρικήΔιεύθυνση
        {
            get { return smartOidΚεντρικήΔιεύθυνση; }
            set { SetPropertyValue(nameof(SmartOidΚεντρικήΔιεύθυνση), ref smartOidΚεντρικήΔιεύθυνση, value); }
        }
        Guid smartOidΚεντρικήΔιεύθυνση;

        public string Κωδικός
        {
            get { return κωδικός; }
            set { SetPropertyValue(nameof(Κωδικός), ref κωδικός, value); }
        }
        string κωδικός;

        public string Επωνυμία
        {
            get { return επωνυμία; }
            set { SetPropertyValue(nameof(Επωνυμία), ref επωνυμία, value); }
        }
        string επωνυμία;
        public string Διακριτικόςτίτλος
        {
            get { return διακριτικόςτίτλος; }
            set { SetPropertyValue(nameof(Διακριτικόςτίτλος), ref διακριτικόςτίτλος, value); }
        }
        string διακριτικόςτίτλος;

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

        public string Email
        {
            get { return email; }
            set { SetPropertyValue(nameof(Email), ref email, value); }
        }
        string email;
        public bool CanUpload
        {
            get { return canUpload; }
            set { SetPropertyValue(nameof(CanUpload), ref canUpload, value); }
        }
        bool canUpload;
        public string ImageName
        {
            get { return imageName; }
            set { SetPropertyValue(nameof(ImageName), ref imageName, value); }
        }
        string imageName;
        public string ImageBytes
        {
            get { return imageBytes; }
            set { SetPropertyValue(nameof(ImageBytes), ref imageBytes, value); }
        }
        string imageBytes;
        public string Σημείωση1
        {
            get { return σημείωση1; }
            set { SetPropertyValue(nameof(Σημείωση1), ref σημείωση1, value); }
        }
        string σημείωση1;
        public string Σημείωση2
        {
            get { return σημείωση2; }
            set { SetPropertyValue(nameof(Σημείωση2), ref σημείωση2, value); }
        }
        string σημείωση2;
        [Association]
        public Πωλητής Πωλητής
        {
            get { return πωλητής; }
            set { SetPropertyValue(nameof(Πωλητής), ref πωλητής, value); }
        }
        Πωλητής πωλητής;
        public ΔιευθύνσειςΠελάτη ΚεντρικήΔιευθυνση
        {
            get { return κεντρικήδιευθυνση; }
            set { SetPropertyValue(nameof(ΚεντρικήΔιευθυνση), ref κεντρικήδιευθυνση, value); }
        }
        ΔιευθύνσειςΠελάτη κεντρικήδιευθυνση;

        [Association,Aggregated]
        public XPCollection<ΔιευθύνσειςΠελάτη> ΔιευθύνσειςΠελάτη
        {
            get { return GetCollection<ΔιευθύνσειςΠελάτη>(nameof(ΔιευθύνσειςΠελάτη)); }
            
        }
        [Association]
        public XPCollection<ΠαραστατικάΠωλήσεων> ΠαραστατικάΠωλήσεωνΠελάτη
        {
            get { return GetCollection<ΠαραστατικάΠωλήσεων>(nameof(ΠαραστατικάΠωλήσεωνΠελάτη)); }
        }

        [Association]
        public XPCollection<ΠαραστατικάΕισπράξεων> ΠαραστατικάΕισπράξεων
        {
            get { return GetCollection<ΠαραστατικάΕισπράξεων>(nameof(ΠαραστατικάΕισπράξεων)); }
        }
        public DateTime ΗμνίαΔημ
        {
            get { return ημνίαδημ; }
            set { SetPropertyValue(nameof(ΗμνίαΔημ), ref ημνίαδημ, value); }
        }
        DateTime ημνίαδημ;
        [NonPersistent]
        public string Addresstring
        {
            get
            {
                var returnString = "";
                if (this.ΚεντρικήΔιευθυνση != null)
                {
                    returnString = this.ΚεντρικήΔιευθυνση.Οδός + " " + this.ΚεντρικήΔιευθυνση.Αριθμός + " ";
                    if (this.ΚεντρικήΔιευθυνση.Πόλη != null)
                    {
                        returnString += this.ΚεντρικήΔιευθυνση.Πόλη.ΟνομαΠόλης + " ";
                    }
                    if(this.ΚεντρικήΔιευθυνση.ΤΚ != null)
                    {
                        returnString+= this.ΚεντρικήΔιευθυνση.ΤΚ.Ονοματκ;
                    }
                }
                return returnString;
            }
        } 

        [NonPersistent]
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(this.Διακριτικόςτίτλος))
                {
                    return this.Επωνυμία;
                }
                else
                {
                    return this.Διακριτικόςτίτλος;
                }
            }
        }
       [NonPersistent]
        public string DoyOid { 
            get 
            {
                string dd;
                dd = ΔΟΥ == null ? "" : ΔΟΥ.SmartOid.ToString();
                return dd;
            }
        }

        public string ToJson()
        {
            string addressJson = "[";
            foreach (var item in ΔιευθύνσειςΠελάτη)
            {
                addressJson += item.ToJson() + ",";
            }
            addressJson = addressJson.Remove(addressJson.Count() - 1);
            addressJson += "]";
            string json = string.Empty;
            json = "{ \"Oid\":\"" + smartOid +
                "\",\"Κωδικός\":\"" + (string.IsNullOrEmpty(Κωδικός) ? "" : Κωδικός) +
                "\",\"Επωνυμία\":\"" + Επωνυμία +
                 "\",\"ΔιακριτικόςΤίτλος\":\"" + (string.IsNullOrEmpty(Διακριτικόςτίτλος) ? "" : Διακριτικόςτίτλος) +
                 "\",\"ΚατηγορίαΦΠΑ\":\"" + ΚατηγορίαΦΠΑ +
                 "\",\"ΑΦΜ\":\"" + ΑΦΜ +
                 "\",\"ΔΟΥ\":\"" + DoyOid +
                 "\",\"Email\":\"" + (string.IsNullOrEmpty(Email) ? "" : Email) +
                 "\",\"Σημείωση1\":\"" + (string.IsNullOrEmpty(Σημείωση1) ? "" : Σημείωση1) +
                 "\",\"Σημείωση2\":\"" + (string.IsNullOrEmpty(Σημείωση2) ? "" : Σημείωση2) +
                "\",\"Πωλητής\":\"" + Πωλητής.SmartOid +
                "\",\"ΚεντρικήΔιευθυνση\":\"" + ΚεντρικήΔιευθυνση.SmartOid +
                "\",\"ΗμνίαΔημ\":\"" + ΗμνίαΔημ.ToString("dd/MM/yyyy HH:mm:ss") +
                "\",\"ΔιευθύνσειςΠελάτη\":" + addressJson +
                ",\"Image\":\"" + (string.IsNullOrEmpty(ImageName) ? "" : ImageName) +
                "\"}";
            return json;
        }
    }

}
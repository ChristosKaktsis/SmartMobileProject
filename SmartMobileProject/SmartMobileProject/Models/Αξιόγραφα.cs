using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class Αξιόγραφα : XPObject
    {
        public Αξιόγραφα() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Αξιόγραφα(Session session) : base(session)
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
        public string Αιτιολογία
        {
            get { return αιτιολογία; }
            set { SetPropertyValue(nameof(Αιτιολογία), ref αιτιολογία, value); }
        }
        string αιτιολογία;
        public decimal Αξία
        {
            get { return αξία; }
            set { SetPropertyValue(nameof(Αξία), ref αξία, value); }
        }
        decimal αξία;
        public string ΑριθμόςΑξιογράφου
        {
            get { return αριθμόςαξιογράφου; }
            set { SetPropertyValue(nameof(ΑριθμόςΑξιογράφου), ref αριθμόςαξιογράφου, value); }
        }
        string αριθμόςαξιογράφου;
        public string Εκδότης
        {
            get { return εκδότης; }
            set { SetPropertyValue(nameof(Εκδότης), ref εκδότης, value); }
        }
        string εκδότης;
        public DateTime ΗμνίαΕκδοσης
        {
            get { return ημνίαεκδοσης; }
            set { SetPropertyValue(nameof(ΗμνίαΕκδοσης), ref ημνίαεκδοσης, value); }
        }
        DateTime ημνίαεκδοσης;
        public DateTime ΗμνίαΛήξης
        {
            get { return ημνίαλήξης; }
            set { SetPropertyValue(nameof(ΗμνίαΛήξης), ref ημνίαλήξης, value); }
        }
        DateTime ημνίαλήξης;
        public DateTime ΗμνίαΠαραλαβής
        {
            get { return ημνίαπαραλαβής; }
            set { SetPropertyValue(nameof(ΗμνίαΠαραλαβής), ref ημνίαπαραλαβής, value); }
        }
        DateTime ημνίαπαραλαβής;
        public string Παραστατικό
        {
            get { return παραστατικό; }
            set { SetPropertyValue(nameof(Παραστατικό), ref παραστατικό, value); }
        }
        string παραστατικό;
        public Τράπεζα ΤράπεζαΕκδοσης
        {
            get { return τράπεζαεκδοσης; }
            set { SetPropertyValue(nameof(ΤράπεζαΕκδοσης), ref τράπεζαεκδοσης, value); }
        }
        Τράπεζα τράπεζαεκδοσης;
        public bool IsUploaded
        {
            get { return canUpload; }
            set { SetPropertyValue(nameof(IsUploaded), ref canUpload, value); }
        }
        bool canUpload;
        [NonPersistent]
        string trapeza
        {
            get
            {
                string t = "";
                if (ΤράπεζαΕκδοσης != null)
                    t = ΤράπεζαΕκδοσης.SmartOid.ToString();
                return t;
            }
        }
        public string ToJson()
        {
            string json = string.Empty;
            json = "{ \"Oid\":\"" + smartOid +
                "\",\"Αιτιολογία\":\"" + Αιτιολογία +
                "\",\"Αξία\":\"" + Αξία +
                "\",\"ΑριθμόςΑξιογράφου\":\"" + ΑριθμόςΑξιογράφου +
                "\",\"Εκδότης\":\"" + (string.IsNullOrEmpty(Εκδότης) ? "" : Εκδότης) +
                "\",\"ΗμνίαΕκδοσης\":\"" + (string.IsNullOrEmpty(ΗμνίαΕκδοσης.ToString()) ? "" : ΗμνίαΕκδοσης.ToString("dd/MM/yyyy HH:mm:ss")) +
                "\",\"ΗμνίαΛήξης\":\"" + (string.IsNullOrEmpty(ΗμνίαΛήξης.ToString()) ? "" : ΗμνίαΛήξης.ToString("dd/MM/yyyy HH:mm:ss")) +
                "\",\"ΗμνίαΠαραλαβής\":\"" + (string.IsNullOrEmpty(ΗμνίαΠαραλαβής.ToString()) ? "" : ΗμνίαΠαραλαβής.ToString("dd/MM/yyyy HH:mm:ss")) +
                "\",\"Παραστατικό\":\"" + (string.IsNullOrEmpty(Παραστατικό) ? "" : Παραστατικό) +
                "\",\"ΤράπεζαΕκδοσης\":\"" + trapeza +
                "\"}";
            return json;
        }

    }

}
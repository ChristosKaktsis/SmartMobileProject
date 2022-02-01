using DevExpress.Xpo;
using System;
using System.Linq;

namespace SmartMobileProject.Models
{
    public class ΠαραστατικάΠωλήσεων : XPObject
    {
        public ΠαραστατικάΠωλήσεων() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΠαραστατικάΠωλήσεων(Session session) : base(session)
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

        public DateTime Ημνία
        {
            get { return ημνία; }
            set { SetPropertyValue(nameof(Ημνία), ref ημνία, value); }
        }
        DateTime ημνία;

        public ΣειρέςΠαραστατικώνΠωλήσεων Σειρά
        {
            get { return σειρά; }
            set { SetPropertyValue(nameof(Σειρά), ref σειρά, value); }
        }
        ΣειρέςΠαραστατικώνΠωλήσεων σειρά;


        public string Παραστατικό
        {
            get { return παραστατικό; }
            set { SetPropertyValue(nameof(Παραστατικό), ref παραστατικό, value); }
        }
        string παραστατικό;
        public bool IsUploaded
        {
            get { return canUpload; }
            set { SetPropertyValue(nameof(IsUploaded), ref canUpload, value); }
        }
        bool canUpload;

        [Association]
        public Πελάτης Πελάτης
        {
            get { return πελάτης; }
            set { SetPropertyValue(nameof(Πελάτης), ref πελάτης, value); }
        }
        Πελάτης πελάτης;

        public ΔιευθύνσειςΠελάτη ΔιεύθυνσηΠαράδοσης
        {
            get { return διεύθυνσηπαράδοσης; }
            set { SetPropertyValue(nameof(ΔιεύθυνσηΠαράδοσης), ref διεύθυνσηπαράδοσης, value); }
        }
        ΔιευθύνσειςΠελάτη διεύθυνσηπαράδοσης;

        [Association]
        public Πωλητής Πωλητής
        {
            get { return πωλητής; }
            set { SetPropertyValue(nameof(Πωλητής), ref πωλητής, value); }
        }
        Πωλητής πωλητής;


        public ΤρόποςΠληρωμής ΤρόποςΠληρωμής
        {
            get { return τρόποςπληρωμής; }
            set { SetPropertyValue(nameof(ΤρόποςΠληρωμής), ref τρόποςπληρωμής, value); }
        }
        ΤρόποςΠληρωμής τρόποςπληρωμής;

        public ΤρόποςΑποστολής ΤρόποςΑποστολής
        {
            get { return τρόποςαποστολής; }
            set { SetPropertyValue(nameof(ΤρόποςΑποστολής), ref τρόποςαποστολής, value); }
        }
        ΤρόποςΑποστολής τρόποςαποστολής;


        [Association]
        public XPCollection<ΓραμμέςΠαραστατικώνΠωλήσεων> ΓραμμέςΠαραστατικώνΠωλήσεων
        {
            get { return GetCollection<ΓραμμέςΠαραστατικώνΠωλήσεων>(nameof(ΓραμμέςΠαραστατικώνΠωλήσεων)); }

        }

        public Int32 Ποσότητα
        {
            get { return ποσότητα; }
            set { SetPropertyValue(nameof(Ποσότητα), ref ποσότητα, value); }
        }
        Int32 ποσότητα;

        public decimal ΚαθαρήΑξία
        {
            get { return καθαρήαξία; }
            set { SetPropertyValue(nameof(ΚαθαρήΑξία), ref καθαρήαξία, value); }
        }
        decimal καθαρήαξία;

        public decimal ΑξίαΕκπτωσης
        {
            get { return αξίαεκπτωσης; }
            set { SetPropertyValue(nameof(ΑξίαΕκπτωσης), ref αξίαεκπτωσης, value); }
        }
        decimal αξίαεκπτωσης;

        public decimal Φπα
        {
            get { return φπα; }
            set { SetPropertyValue(nameof(Φπα), ref φπα, value); }
        }
        decimal φπα;

        public decimal ΑξίαΠαραστατικού
        {
            get { return αξίαπαραστατικού; }
            set { SetPropertyValue(nameof(ΑξίαΠαραστατικού), ref αξίαπαραστατικού, value); }
        }
        decimal αξίαπαραστατικού;
        public DateTime ΗμνίαΔημ
        {
            get { return ημνίαδημ; }
            set { SetPropertyValue(nameof(ΗμνίαΔημ), ref ημνίαδημ, value); }
        }
        DateTime ημνίαδημ;
        public string Σχολια
        {
            get { return σχολια; }
            set { SetPropertyValue(nameof(Σχολια), ref σχολια, value); }
        }
        string σχολια;
        [NonPersistent]
        string seira
        {
            get
            {
                if (Σειρά != null)
                    return Σειρά.ToJson();
                return string.Empty;
            }
        }
        [NonPersistent]
        string dieuthpar
        {
            get
            {
                if (ΔιεύθυνσηΠαράδοσης != null)
                    return ΔιεύθυνσηΠαράδοσης.SmartOid.ToString();
                return string.Empty;
            }
        }
        [NonPersistent]
        string politis
        {
            get
            {
                if (Πωλητής != null)
                    return Πωλητής.SmartOid.ToString();
                return string.Empty;
            }
        }
        [NonPersistent]
        string tropospliromis
        {
            get
            {
                if (ΤρόποςΠληρωμής != null)
                    return ΤρόποςΠληρωμής.SmartOid.ToString();
                return string.Empty;
            }
        }
        [NonPersistent]
        string troposapostolis
        {
            get
            {
                if (ΤρόποςΑποστολής != null)
                    return ΤρόποςΑποστολής.SmartOid.ToString();
                return string.Empty;
            }
        }
        public string ToJson()
        {
            string LineJson = "[";
            foreach (var item in ΓραμμέςΠαραστατικώνΠωλήσεων)
            {
                LineJson += item.ToJson() + ",";
            }
            LineJson = LineJson.Remove(LineJson.Count() - 1);
            LineJson += "]";
            string json = string.Empty;
            json = "{ \"Oid\":\"" + smartOid +
                "\",\"ΗμνίαΔημιουργίας\":\"" + (string.IsNullOrEmpty(Ημνία.ToString()) ? "" : Ημνία.ToString("dd/MM/yyyy HH:mm:ss")) +
                "\",\"Σειρά\":" + seira +
                ",\"Παραστατικό\":\"" + (string.IsNullOrEmpty(Παραστατικό) ? "" : Παραστατικό) +
                "\",\"Πελάτης\":\"" + Πελάτης.SmartOid +
                "\",\"ΔιεύθυνσηΠαράδοσης\":\"" + dieuthpar +
                "\",\"Πωλητής\":\"" + politis +
                "\",\"ΤρόποςΠληρωμής\":\"" + tropospliromis +
                "\",\"ΤρόποςΑποστολής\":\"" + troposapostolis +
                "\",\"ΗμνίαΔημ\":\"" + ΗμνίαΔημ.ToString("dd/MM/yyyy HH:mm:ss") +
                "\",\"Σχόλια\":\"" + (string.IsNullOrEmpty(Σχολια) ? "" : Σχολια) +
                "\",\"ΓραμμέςΠαραστατικώνΠωλήσεων\":" + LineJson +
                "}";
            return json;
        }
    }

}
using DevExpress.Xpo;
using System;
using System.Linq;

namespace SmartMobileProject.Models
{
    public class ΠαραστατικάΕισπράξεων : XPObject
    {
        public ΠαραστατικάΕισπράξεων() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΠαραστατικάΕισπράξεων(Session session) : base(session)
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

        public ΣειρέςΠαραστατικώνΕισπράξεων Σειρά
        {
            get { return σειρά; }
            set { SetPropertyValue(nameof(Σειρά), ref σειρά, value); }
        }
        ΣειρέςΠαραστατικώνΕισπράξεων σειρά;

        public string Παραστατικό
        {
            get { return παραστατικό; }
            set { SetPropertyValue(nameof(Παραστατικό), ref παραστατικό, value); }
        }
        string παραστατικό;
        public string Αιτιολογία
        {
            get { return αιτιολογία; }
            set { SetPropertyValue(nameof(Αιτιολογία), ref αιτιολογία, value); }
        }
        string αιτιολογία;
        public decimal Χρέωση
        {
            get { return χρέωση; }
            set { SetPropertyValue(nameof(Χρέωση), ref χρέωση, value); }
        }
        decimal χρέωση;
        public decimal Πίστωση
        {
            get { return πίστωση; }
            set { SetPropertyValue(nameof(Πίστωση), ref πίστωση, value); }
        }
        decimal πίστωση;
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

        public ΔιευθύνσειςΠελάτη ΔιεύθυνσηΕίσπραξης
        {
            get { return διεύθυνσηείσπραξης; }
            set { SetPropertyValue(nameof(ΔιεύθυνσηΕίσπραξης), ref διεύθυνσηείσπραξης, value); }
        }
        ΔιευθύνσειςΠελάτη διεύθυνσηείσπραξης;

        [Association]
        public Πωλητής Πωλητής
        {
            get { return πωλητής; }
            set { SetPropertyValue(nameof(Πωλητής), ref πωλητής, value); }
        }
        Πωλητής πωλητής;

        [Association]
        public XPCollection<ΓραμμέςΠαραστατικώνΕισπράξεων> ΓραμμέςΠαραστατικώνΕισπράξεων
        {
            get { return GetCollection<ΓραμμέςΠαραστατικώνΕισπράξεων>(nameof(ΓραμμέςΠαραστατικώνΕισπράξεων)); }

        }
        public DateTime ΗμνίαΔημ
        {
            get { return ημνίαδημ; }
            set { SetPropertyValue(nameof(ΗμνίαΔημ), ref ημνίαδημ, value); }
        }
        DateTime ημνίαδημ;
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
        string politis
        {
            get
            {
                if (Πωλητής != null)
                    return Πωλητής.SmartOid.ToString();
                return string.Empty;
            }
        }
        public string ToJson()
        {
            string LineJson = "[";
            foreach (var item in ΓραμμέςΠαραστατικώνΕισπράξεων)
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
                "\",\"Αιτιολογία\":\"" + (string.IsNullOrEmpty(Αιτιολογία) ? "" : Αιτιολογία) +
                "\",\"Πωλητής\":\"" + politis +
                "\",\"ΗμνίαΔημ\":\"" + ΗμνίαΔημ.ToString("dd/MM/yyyy HH:mm:ss") +
                "\",\"ΓραμμέςΠαραστατικώνΕισπράξεων\":" + LineJson +
                "}";
            return json;
        }
    }

}
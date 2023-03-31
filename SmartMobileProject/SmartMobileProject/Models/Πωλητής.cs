using DevExpress.Xpo;
using Newtonsoft.Json;
using System;

namespace SmartMobileProject.Models
{
    public class Πωλητής : XPObject
    {
        public Πωλητής() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Πωλητής(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        [Indexed(Unique = true)]
        [JsonProperty("Oid")]
        public Guid SmartOid
        {
            get { return smartOid; }
            set { SetPropertyValue(nameof(SmartOid), ref smartOid, value); }
        }
        Guid smartOid;

        public string Ονοματεπώνυμο
        {
            get { return ονοματεπώνυμο; }
            set { SetPropertyValue(nameof(Ονοματεπώνυμο), ref ονοματεπώνυμο, value); }
        }
        string ονοματεπώνυμο;

        public string Email
        {
            get { return email; }
            set { SetPropertyValue(nameof(Email), ref email, value); }
        }
        string email;

        public string FAX
        {
            get { return fAX; }
            set { SetPropertyValue(nameof(FAX), ref fAX, value); }
        }
        string fAX;

        public string KίνΤηλέφωνο
        {
            get { return κίνΤηλέφωνο; }
            set { SetPropertyValue(nameof(KίνΤηλέφωνο), ref κίνΤηλέφωνο, value); }
        }
        string κίνΤηλέφωνο;

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

        [Association]
        public XPCollection<Πελάτης> Πελάτες
        {
            get { return GetCollection<Πελάτης>(nameof(Πελάτες)); }
        }
        [Association]
        public XPCollection<ΠαραστατικάΠωλήσεων> Παραστατικόπωλήσεων
        {
            get { return GetCollection<ΠαραστατικάΠωλήσεων>(nameof(Παραστατικόπωλήσεων)); }
        }
        [Association]
        public XPCollection<ΠαραστατικάΕισπράξεων> ΠαραστατικάΕισπράξεων
        {
            get { return GetCollection<ΠαραστατικάΕισπράξεων>(nameof(ΠαραστατικάΕισπράξεων)); }
        }
        [Association]
        public XPCollection<Εργασία> Εργασίες
        {
            get { return GetCollection<Εργασία>(nameof(Εργασίες)); }
        }
    }

}
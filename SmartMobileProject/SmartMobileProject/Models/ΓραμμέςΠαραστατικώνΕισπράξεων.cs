using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class ΓραμμέςΠαραστατικώνΕισπράξεων : XPObject
    {
        public ΓραμμέςΠαραστατικώνΕισπράξεων() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΓραμμέςΠαραστατικώνΕισπράξεων(Session session) : base(session)
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

        public ΛογαριασμοίΧρηματικώνΔιαθέσιμων Λογαριασμός
        {
            get { return λογαριασμός; }
            set { SetPropertyValue(nameof(Λογαριασμός), ref λογαριασμός, value); }
        }
        ΛογαριασμοίΧρηματικώνΔιαθέσιμων λογαριασμός;

        public decimal Ποσόν
        {
            get { return ποσόν; }
            set { SetPropertyValue(nameof(Ποσόν), ref ποσόν, value); }
        }
        decimal ποσόν;
        public Αξιόγραφα Αξιόγραφα
        {
            get { return αξιόγραφα; }
            set { SetPropertyValue(nameof(Αξιόγραφα), ref αξιόγραφα, value); }
        }
        Αξιόγραφα αξιόγραφα;
        public bool IsUploaded
        {
            get { return canUpload; }
            set { SetPropertyValue(nameof(IsUploaded), ref canUpload, value); }
        }
        bool canUpload;

        [Association]
        public ΠαραστατικάΕισπράξεων ΠαραστατικάΕισπράξεων
        {
            get { return παραστατικάεισπράξεων; }
            set { SetPropertyValue(nameof(ΠαραστατικάΕισπράξεων), ref παραστατικάεισπράξεων, value); }
        }
        ΠαραστατικάΕισπράξεων παραστατικάεισπράξεων;

        [NonPersistent]
        public string DisplayName
        {
            get
            {
                if (Λογαριασμός == null)
                {
                    return "Αξιόγραφο :"+ Αξιόγραφα.ΑριθμόςΑξιογράφου;
                }
                else
                {
                    return "Λογαριασμός :" + Λογαριασμός.Λογαριασμός;
                }
            }
        }
        [NonPersistent]
        string logariasmos
        {
            get
            {
                if (Λογαριασμός != null)
                    return Λογαριασμός.SmartOid.ToString();
                return string.Empty;
            }
        }
        [NonPersistent]
        string axiografo
        {
            get
            {
                if (Αξιόγραφα != null)
                    return Αξιόγραφα.SmartOid.ToString();
                return string.Empty;
            }
        }
        public string ToJson()
        {
            string json = string.Empty;
            json = "{ \"Oid\":\"" + smartOid +
                "\",\"ΠαραστατικάΕισπράξεων\":\"" + ΠαραστατικάΕισπράξεων.SmartOid +
                "\",\"Λογαριασμός\":\"" + logariasmos +
                "\",\"Αξιόγραφο\":\"" + axiografo +
                "\",\"Ποσόν\":\"" + (string.IsNullOrEmpty(Ποσόν.ToString()) ?"": Ποσόν.ToString()) +
                "\"}";
            return json;
        }
    }

}
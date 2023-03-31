using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class ΣειρέςΠαραστατικώνΕισπράξεων : XPObject
    {
        public ΣειρέςΠαραστατικώνΕισπράξεων() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΣειρέςΠαραστατικώνΕισπράξεων(Session session) : base(session)
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

        public string Σειρά
        {
            get { return σειρά; }
            set { SetPropertyValue(nameof(Σειρά), ref σειρά, value); }
        }
        string σειρά;

        public string Περιγραφή
        {
            get { return περιγραφή; }
            set { SetPropertyValue(nameof(Περιγραφή), ref περιγραφή, value); }
        }
        string περιγραφή;
        public Guid ΠρόθεμαΑρίθμησης
        {
            get { return πρόθεμαΑρίθμησης; }
            set { SetPropertyValue(nameof(ΠρόθεμαΑρίθμησης), ref πρόθεμαΑρίθμησης, value); }
        }
        Guid πρόθεμαΑρίθμησης;
        public int Counter
        {
            get { return counter; }
            set { SetPropertyValue(nameof(Counter), ref counter, value); }
        }
        int counter;

        public string ToJson()
        {
            string json = string.Empty;
            json = "{ \"Oid\":\"" + smartOid +
                "\",\"Πρόθεμα\":\"" + (string.IsNullOrEmpty(Σειρά) ? "" : Σειρά) +
                "\",\"Περιγραφή\":\"" + (string.IsNullOrEmpty(Περιγραφή) ? "" : Περιγραφή) +
                "\"}";
            return json;
        }
    }

}
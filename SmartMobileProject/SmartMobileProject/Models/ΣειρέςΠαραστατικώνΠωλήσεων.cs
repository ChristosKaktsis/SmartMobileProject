using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class ΣειρέςΠαραστατικώνΠωλήσεων : XPObject
    {
        public ΣειρέςΠαραστατικώνΠωλήσεων() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΣειρέςΠαραστατικώνΠωλήσεων(Session session) : base(session)
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
        public bool Λιανική
        {
            get { return λιανική; }
            set { SetPropertyValue(nameof(Λιανική), ref λιανική, value); }
        }
        bool λιανική;
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
        public int ΚίνησηΣυναλασόμενου
        {
            get { return κίνησηΣυναλασόμενου; }
            set { SetPropertyValue(nameof(ΚίνησηΣυναλασόμενου), ref κίνησηΣυναλασόμενου, value); }
        }
        int κίνησηΣυναλασόμενου;
        public string PrintType
        {
            get { return printtype; }
            set { SetPropertyValue(nameof(PrintType), ref printtype, value); }
        }
        string printtype;
        public Guid IDΠωλητή
        {
            get { return iDΠωλητή; }
            set { SetPropertyValue(nameof(IDΠωλητή), ref iDΠωλητή, value); }
        }
        Guid iDΠωλητή;
        public string ΣκοπόςΔιακίνησης
        {
            get { return σκοποςΔιακίνησης; }
            set { SetPropertyValue(nameof(ΣκοπόςΔιακίνησης), ref σκοποςΔιακίνησης, value); }
        }
        string σκοποςΔιακίνησης;
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
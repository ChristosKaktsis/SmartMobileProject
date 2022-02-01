using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class ΓραμμέςΠαραστατικώνΠωλήσεων : XPObject
    {
        public ΓραμμέςΠαραστατικώνΠωλήσεων() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΓραμμέςΠαραστατικώνΠωλήσεων(Session session) : base(session)
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

        public Είδος Είδος
        {
            get { return είδος; }
            set { SetPropertyValue(nameof(Είδος), ref είδος, value); }
        }
        Είδος είδος;
        public BarCodeΕίδους BarCodeΕίδους
        {
            get { return barCodeΕίδους; }
            set { SetPropertyValue(nameof(BarCodeΕίδους), ref barCodeΕίδους, value); }
        }
        BarCodeΕίδους barCodeΕίδους;

        public Single Ποσότητα
        {
            get { return ποσότητα; }
            set { SetPropertyValue(nameof(Ποσότητα), ref ποσότητα, value); }
        }
        Single ποσότητα;

        public double Τιμή
        {
            get { return τιμή; }
            set { SetPropertyValue(nameof(Τιμή), ref τιμή, value); }
        }
        double τιμή;

        public decimal ΚαθαρήΑξία
        {
            get { return καθαρήαξία; }
            set { SetPropertyValue(nameof(ΚαθαρήΑξία), ref καθαρήαξία, value); }
        }
        decimal καθαρήαξία;

        public Single Εκπτωση
        {
            get { return εκπτωση; }
            set { SetPropertyValue(nameof(Εκπτωση), ref εκπτωση, value); }
        }
        Single εκπτωση;

        public decimal ΑξίαΕκπτωσης
        {
            get { return αξίαεκπτωσης; }
            set { SetPropertyValue(nameof(ΑξίαΕκπτωσης), ref αξίαεκπτωσης, value); }
        }
        decimal αξίαεκπτωσης;

        public Single ΠοσοστόΦπα
        {
            get { return ποσοστόφπα; }
            set { SetPropertyValue(nameof(ΠοσοστόΦπα), ref ποσοστόφπα, value); }
        }
        Single ποσοστόφπα;

        public decimal Φπα
        {
            get { return φπα; }
            set { SetPropertyValue(nameof(Φπα), ref φπα, value); }
        }
        decimal φπα;

        public decimal ΑξίαΓραμμής
        {
            get { return αξίαγραμμής; }
            set { SetPropertyValue(nameof(ΑξίαΓραμμής), ref αξίαγραμμής, value); }
        }
        decimal αξίαγραμμής;
        public bool IsUploaded
        {
            get { return canUpload; }
            set { SetPropertyValue(nameof(IsUploaded), ref canUpload, value); }
        }
        bool canUpload;
        public string Σχολια
        {
            get { return σχολια; }
            set { SetPropertyValue(nameof(Σχολια), ref σχολια, value); }
        }
        string σχολια;
        [Association]
        public ΠαραστατικάΠωλήσεων ΠαραστατικάΠωλήσεων
        {
            get { return παραστατικάπωλήσεων; }
            set { SetPropertyValue(nameof(ΠαραστατικάΠωλήσεων), ref παραστατικάπωλήσεων, value); }
        }
        ΠαραστατικάΠωλήσεων παραστατικάπωλήσεων;
        [NonPersistent]
        public string BarCodeString 
        { 
            get 
            {
                return BarCodeΕίδους != null ? $"{BarCodeΕίδους.Κωδικός}\n{BarCodeΕίδους.Περιγραφή}\n{BarCodeΕίδους.Χρώμα} {BarCodeΕίδους.Μέγεθος}" : string.Empty;
            } 
        }
        [NonPersistent]
        public string BarCodeCode
        {
            get
            {
                return BarCodeΕίδους != null ? BarCodeΕίδους.Κωδικός: string.Empty;
            }
        }
        [NonPersistent]
        public string BarCodeInfo
        {
            get
            {
                return BarCodeΕίδους != null ? $"{BarCodeΕίδους.Περιγραφή} {BarCodeΕίδους.Χρώμα} {BarCodeΕίδους.Μέγεθος}\n{BarCodeΕίδους.Κωδικός}" : $"{Είδος.Περιγραφή}\n{Είδος.Κωδικός}";
            }
        }
        public string ToJson()
        {
            string json = string.Empty;
            json = "{ \"Oid\":\"" + smartOid +
                "\",\"ΗμνίαΔημιουργίας\":\"" + (string.IsNullOrEmpty(ΠαραστατικάΠωλήσεων.Ημνία.ToString()) ? "" : ΠαραστατικάΠωλήσεων.Ημνία.ToString()) +
                "\",\"ΠαραστατικάΠωλήσεων\":\"" + ΠαραστατικάΠωλήσεων.SmartOid +
                "\",\"Είδος\":\"" + Είδος.SmartOid +
                "\",\"BarCode\":\"" + BarCodeCode +
                "\",\"Ποσότητα\":\"" + Ποσότητα +
                "\",\"Τιμή\":\"" + Τιμή +
                "\",\"ΚαθαρήΑξία\":\"" + ΚαθαρήΑξία +
                "\",\"Εκπτωση\":\"" + Εκπτωση +
                "\",\"ΑξίαΕκπτωσης\":\"" + ΑξίαΕκπτωσης +
                "\",\"ΠοσοστόΦπα\":\"" + ΠοσοστόΦπα +
                "\",\"Φπα\":\"" + Φπα +
                "\",\"ΑξίαΓραμμής\":\"" + ΑξίαΓραμμής +
                "\",\"Σχόλια\":\"" + (string.IsNullOrEmpty(Σχολια) ? "" : Σχολια) +
                "\"}";
            return json;
        }
    }

}
using DevExpress.Xpo;
using Newtonsoft.Json;
using System;
using System.IO;
using Xamarin.Forms;

namespace SmartMobileProject.Models
{
    public class Είδος : XPObject
    {
        public Είδος() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Είδος(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }
        [JsonProperty("Oid")]
        public Guid SmartOid
        {
            get { return smartOid; }
            set { SetPropertyValue(nameof(SmartOid), ref smartOid, value); }
        }
        Guid smartOid;

        public string Κωδικός
        {
            get { return κωδικός; }
            set { SetPropertyValue(nameof(Κωδικός), ref κωδικός, value); }
        }
        string κωδικός;

        public string Περιγραφή
        {
            get { return περιγραφή; }
            set { SetPropertyValue(nameof(Περιγραφή), ref περιγραφή, value); }
        }
        string περιγραφή;

        public ΦΠΑ ΦΠΑ
        {
            get { return φπα; }
            set { SetPropertyValue(nameof(ΦΠΑ), ref φπα, value); }
        }
        ΦΠΑ φπα;

        //start
        //Ομαδοποιησεις
        //
        public ΟικογένειαΕίδους Οικογένεια
        {
            get { return οικογένεια; }
            set { SetPropertyValue(nameof(Οικογένεια), ref οικογένεια, value); }
        }
        ΟικογένειαΕίδους οικογένεια;

        public ΥποοικογένειαΕίδους Υποοικογένεια
        {
            get { return υποοικογένεια; }
            set { SetPropertyValue(nameof(Υποοικογένεια), ref υποοικογένεια, value); }
        }
        ΥποοικογένειαΕίδους υποοικογένεια;


        public ΟμάδαΕίδους Ομάδα
        {
            get { return ομάδα; }
            set { SetPropertyValue(nameof(Ομάδα), ref ομάδα, value); }
        }
        ΟμάδαΕίδους ομάδα;

        public ΚατηγορίαΕίδους Κατηγορία
        {
            get { return κατηγορία; }
            set { SetPropertyValue(nameof(Κατηγορία), ref κατηγορία, value); }
        }
        ΚατηγορίαΕίδους κατηγορία;

        //end
        //ομαδοποιησεις
        //

        public double ΤιμήΧονδρικής
        {
            get { return τιμήχονδρικής; }
            set { SetPropertyValue(nameof(ΤιμήΧονδρικής), ref τιμήχονδρικής, value); }
        }
        double τιμήχονδρικής;
        public double ΤιμήΛιανικής
        {
            get { return τιμήΛιανικής; }
            set { SetPropertyValue(nameof(ΤιμήΛιανικής), ref τιμήΛιανικής, value); }
        }
        double τιμήΛιανικής;
        public DateTime ΗμνίαΔημ
        {
            get { return ημνίαδημ; }
            set { SetPropertyValue(nameof(ΗμνίαΔημ), ref ημνίαδημ, value); }
        }
        DateTime ημνίαδημ;

        [Association, Aggregated]
        public XPCollection<BarCodeΕίδους> BarCodeΕίδους
        {
            get { return GetCollection<BarCodeΕίδους>(nameof(BarCodeΕίδους)); }
        }
        //
        private byte[] imageBytes;
        public byte[] ImageBytes 
        { 
            get => imageBytes; 
            set => SetPropertyValue(nameof(ImageBytes), ref imageBytes, value);
        }
        [NonPersistent]
        public ImageSource ImageSource { get => ImageBytes != null && ImageBytes.Length > 8 ? ImageSource.FromStream(() => new MemoryStream(ImageBytes)) : "icon_about.png"; }
        [NonPersistent]
        Single ποσότητα;
        public Single Ποσότητα
        {
            get
            {
                return ποσότητα;
            }
            set { SetPropertyValue(nameof(Ποσότητα), ref ποσότητα, value); }
        }
        public double getPrice(bool Λιανικής) 
        {
            return Λιανικής ? ΤιμήΛιανικής : ΤιμήΧονδρικής;
        }
        /// <summary>
        /// Για να εμφανισουμε τιμη λιανικης ή χονδρικής στην επιλογή είδους στο παρπωλ
        /// Καθολου καλο practice δεν θα πρεπει να γινει με αυτον τον τροπο 
        /// Θα πρεπει να αλλαξει το μοντελο και ο τροπος που φορτονονται τα δεδομενα για να γινει σωστα
        /// </summary>
        public double getTimh 
        {
            get
            {
                if (ViewModels.DocHelperViewModel.Order == null)
                    return ΤιμήΧονδρικής;
                if (ViewModels.DocHelperViewModel.Order.Σειρά.Λιανική)
                    return ΤιμήΛιανικής;
                return ΤιμήΧονδρικής;
            }
        }

    }

}
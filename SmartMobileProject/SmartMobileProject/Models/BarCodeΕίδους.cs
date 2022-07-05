using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    [Persistent]
    public class BarCodeΕίδους : BaseModel
    {
        
        [Key]
        public string Κωδικός { get; set; }
        
        public bool ΑκολουθείΤήνΤιμήΕίδους { get; set; }

        public string Περιγραφή { get; set; }

        public double ΤιμήΧονδρικής { get; set; }
        public double ΤιμήΛιανικής { get; set; }


        public Guid ΕίδοςOid { get; set; }

        public string Χρώμα { get; set; }

        public string Μέγεθος { get; set; }

        [Association]
        public Είδος Είδος { get; set; }

        public ΦΠΑ ΦΠΑ { get; set; }

        [NonPersistent]
        public float Ποσότητα
        {
            get { return ποσότητα; }
            set { SetProperty(ref ποσότητα, value); }
        }
        private float ποσότητα;
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
                if (ViewModels.ΝέοΠαραστατικόViewModel.Order == null)
                    return ΤιμήΧονδρικής;
                if (ViewModels.ΝέοΠαραστατικόViewModel.Order.Σειρά.Λιανική)
                    return ΤιμήΛιανικής;
                return ΤιμήΧονδρικής;
            }
        }
    }

}
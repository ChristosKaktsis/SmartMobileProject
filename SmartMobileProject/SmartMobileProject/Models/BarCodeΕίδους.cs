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
    }

}
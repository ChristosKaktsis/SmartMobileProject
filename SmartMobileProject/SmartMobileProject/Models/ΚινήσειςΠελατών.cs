using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class ΚινήσειςΠελατών : XPObject
    {
        public ΚινήσειςΠελατών() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΚινήσειςΠελατών(Session session) : base(session)
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
        public string Παραστατικό
        {
            get { return παραστατικό; }
            set { SetPropertyValue(nameof(Παραστατικό), ref παραστατικό, value); }
        }
        string παραστατικό;
        public string Πελάτης
        {
            get { return πελάτης; }
            set { SetPropertyValue(nameof(Πελάτης), ref πελάτης, value); }
        }
        string πελάτης;
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
        public decimal Υπόλοιπο
        {
            get { return υπόλοιπο; }
            set { SetPropertyValue(nameof(Υπόλοιπο), ref υπόλοιπο, value); }
        }
        decimal υπόλοιπο;
        public decimal ΠροοδευτικόΥπόλοιπο
        {
            get { return προοδευτικόΥπόλοιπο; }
            set { SetPropertyValue(nameof(ΠροοδευτικόΥπόλοιπο), ref προοδευτικόΥπόλοιπο, value); }
        }
        decimal προοδευτικόΥπόλοιπο;
    }

}
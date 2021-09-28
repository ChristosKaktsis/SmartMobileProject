using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class Πόλη : XPObject
    {
        public Πόλη() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Πόλη(Session session) : base(session)
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

        public string ΟνομαΠόλης
        {
            get { return ονομαπόλης; }
            set { SetPropertyValue(nameof(ΟνομαΠόλης), ref ονομαπόλης, value); }
        }
        string ονομαπόλης;

        public double ΓεωγραφικόΜήκος
        {
            get { return γεωγραφικόΜήκος; }
            set { SetPropertyValue(nameof(ΓεωγραφικόΜήκος), ref γεωγραφικόΜήκος, value); }
        }
        double γεωγραφικόΜήκος;

        public double ΓεωγραφικόΠλάτος
        {
            get { return γεωγραφικόΠλάτος; }
            set { SetPropertyValue(nameof(ΓεωγραφικόΠλάτος), ref γεωγραφικόΠλάτος, value); }
        }
        double γεωγραφικόΠλάτος;
    }

}
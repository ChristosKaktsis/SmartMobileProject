using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class ΦΠΑ : XPObject
    {
        public ΦΠΑ() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΦΠΑ(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }
        public string Φπαid
        {
            get { return φπαid; }
            set { SetPropertyValue(nameof(Φπαid), ref φπαid, value); }
        }
        string φπαid;

        public string Ομάδαφπα
        {
            get { return ομάδαφπα; }
            set { SetPropertyValue(nameof(Ομάδαφπα), ref ομάδαφπα, value); }
        }
        string ομάδαφπα;

        public Single Φπακανονικό
        {
            get { return φπακανονικό; }
            set { SetPropertyValue(nameof(Φπακανονικό), ref φπακανονικό, value); }
        }
        Single φπακανονικό;

        public Single Φπαμειωμένο
        {
            get { return φπαμειωμένο; }
            set { SetPropertyValue(nameof(Φπαμειωμένο), ref φπαμειωμένο, value); }
        }
        Single φπαμειωμένο;

        public Single Φπαεξαίρεση
        {
            get { return φπαεξαίρεση; }
            set { SetPropertyValue(nameof(Φπαεξαίρεση), ref φπαεξαίρεση, value); }
        }
        Single φπαεξαίρεση;
    }

}
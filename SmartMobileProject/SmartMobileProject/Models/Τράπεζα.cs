using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class Τράπεζα : XPObject
    {
        public Τράπεζα() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Τράπεζα(Session session) : base(session)
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
        public string ΟνομαΤράπεζας
        {
            get { return ονοματράπεζας; }
            set { SetPropertyValue(nameof(ΟνομαΤράπεζας), ref ονοματράπεζας, value); }
        }
        string ονοματράπεζας;
    }

}
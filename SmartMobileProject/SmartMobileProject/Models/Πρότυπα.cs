using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class Πρότυπα : XPObject
    {
        public Πρότυπα() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Πρότυπα(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }
        public string Περιγραφή
        {
            get { return περιγραφή; }
            set { SetPropertyValue(nameof(Περιγραφή), ref περιγραφή, value); }
        }
        string περιγραφή;
    }

}
using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class ΙδιότηταΕνέργειας : XPObject
    {
        public ΙδιότηταΕνέργειας() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΙδιότηταΕνέργειας(Session session) : base(session)
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
        public string Περιγραφή
        {
            get { return περιγραφή; }
            set { SetPropertyValue(nameof(Περιγραφή), ref περιγραφή, value); }
        }
        string περιγραφή;
        public int Τύποςιδιότητας
        {
            get { return τύποςιδιότητας; }
            set { SetPropertyValue(nameof(Τύποςιδιότητας), ref τύποςιδιότητας, value); }
        }
        int τύποςιδιότητας;
        [Association]
        public XPCollection<ΕπιλογήΙδιότητας> ΕπιλογήΙδιότητας
        {
            get { return GetCollection<ΕπιλογήΙδιότητας>(nameof(ΕπιλογήΙδιότητας)); }
        }
    }

}
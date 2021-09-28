using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class ΤαχυδρομικόςΚωδικός : XPObject
    {
        public ΤαχυδρομικόςΚωδικός() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΤαχυδρομικόςΚωδικός(Session session) : base(session)
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

        public string Ονοματκ
        {
            get { return ονοματκ; }
            set { SetPropertyValue(nameof(Ονοματκ), ref ονοματκ, value); }
        }
        string ονοματκ;

        public Πόλη Πόλη
        {
            get { return πόλη; }
            set { SetPropertyValue(nameof(Πόλη), ref πόλη, value); }
        }
        Πόλη πόλη;

        public string Νομός
        {
            get { return νομός; }
            set { SetPropertyValue(nameof(Νομός), ref νομός, value); }
        }
        string νομός;

        public string Περιοχή
        {
            get { return περιοχή; }
            set { SetPropertyValue(nameof(Περιοχή), ref περιοχή, value); }
        }
        string περιοχή;

        public string Χώρα
        {
            get { return χώρα; }
            set { SetPropertyValue(nameof(Χώρα), ref χώρα, value); }
        }
        string χώρα;
    }

}
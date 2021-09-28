﻿using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class ΕπιλογήΙδιότητας : XPObject
    {
        public ΕπιλογήΙδιότητας() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ΕπιλογήΙδιότητας(Session session) : base(session)
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

        [Association]
        public ΙδιότηταΕνέργειας ΙδιότηταΕνέργειας
        {
            get { return ιδιότηταενέργειας; }
            set { SetPropertyValue(nameof(ΙδιότηταΕνέργειας), ref ιδιότηταενέργειας, value); }
        }
        ΙδιότηταΕνέργειας ιδιότηταενέργειας;
    }

}
using DevExpress.Xpo;
using Newtonsoft.Json.Converters;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMobileProject.Core.JsonConverters
{
    internal class AddressHelper : CustomCreationConverter<XPObject>
    {
        DevExpress.Xpo.Session Session { get; }
        public AddressHelper(DevExpress.Xpo.Session session) => this.Session = session ?? throw new ArgumentNullException();
        public override XPObject Create(Type objectType) => new ΔιευθύνσειςΠελάτη(Session);
    }
}

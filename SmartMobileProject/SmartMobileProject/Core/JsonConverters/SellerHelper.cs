using DevExpress.Xpo;
using Newtonsoft.Json.Converters;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMobileProject.Core.JsonConverters
{
    public class SellerHelper : CustomCreationConverter<XPObject>
    {
        DevExpress.Xpo.Session Session { get; }
        public SellerHelper(DevExpress.Xpo.Session session) => this.Session = session ?? throw new ArgumentNullException();
        public override XPObject Create(Type objectType) => new Πωλητής(Session);
    }
}

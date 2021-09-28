using DevExpress.Xpo;
using SmartMobileProject.Core;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace SmartMobileProject.Services
{
    public  class CallAfmInfo
    {


        string xml;
        public string error="";
        ΣτοιχείαΕταιρίας στοιχείαΕταιρίας;
        public CallAfmInfo(ΣτοιχείαΕταιρίας στοιχείαΕταιρίας)
        {
            this.στοιχείαΕταιρίας = στοιχείαΕταιρίας;
        }

        /// <summary>
        /// Δημιουργεί ενα SOAP Request με την μεθοδο του Υπουργείου
        /// </summary>
        /// <returns>Req</returns>
        public  HttpWebRequest CreateSOAPWebRequest()
        {
            //Making Web Request    
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://www1.gsis.gr:443/wsaade/RgWsPublic2/RgWsPublic2");
            //SOAPAction    
            Req.Headers.Add(@"SOAPAction:http://rgwspublic2/RgWsPublic2Service:rgWsPublic2AfmMethod");
            //Content_type    
            Req.ContentType = "application/soap+xml;charset=UTF-8";
            Req.Accept = "text/xml";
            //HTTP method    
            Req.Method = "POST";
            //return HttpWebRequest    
            return Req;
        }

        /// <summary>
        /// Καλεί την CreateSOAPWebRequest(),
        /// Φτιάχνει ενα string Envelope με τα στοιχεία της EXELIXIS 
        /// </summary>
        /// <param name="afmAn">Αφμ για το οποιο θελουμε τα στοιχεία</param>
        /// <returns>Service result</returns>
        public  string InvokeService(string afmAn)
        {
            //Calling CreateSOAPWebRequest method    
            HttpWebRequest request = CreateSOAPWebRequest();
            XmlDocument SOAPReqBody = new XmlDocument();
            //SOAP Body Request  
            SOAPReqBody.LoadXml(@"<env:Envelope xmlns:env=""http://www.w3.org/2003/05/soap-envelope"" xmlns:ns1=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:ns2=""http://rgwspublic2/RgWsPublic2Service"" xmlns:ns3=""http://rgwspublic2/RgWsPublic2"">
                   <env:Header>
                      <ns1:Security>
                         <ns1:UsernameToken>
                            <ns1:Username>"+ στοιχείαΕταιρίας.UsernameΥπηρεσίαςΣτοιχείωνΜητρώου + "</ns1:Username>"+
                            "<ns1:Password>"+στοιχείαΕταιρίας.PasswordΥπηρεσίαςΣτοιχείωνΜητρώου + @"</ns1:Password>
                         </ns1:UsernameToken>
                      </ns1:Security>
                   </env:Header>
                   <env:Body>
                      <ns2:rgWsPublic2AfmMethod>
                         <ns2:INPUT_REC>
                            <ns3:afm_called_by>" + στοιχείαΕταιρίας.ΑΦΜ + @"</ns3:afm_called_by>
                            <ns3:afm_called_for>" + afmAn + @"</ns3:afm_called_for>
                         </ns2:INPUT_REC>
                      </ns2:rgWsPublic2AfmMethod>
                   </env:Body>
                </env:Envelope>");

            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            //Geting response from request    
            using (WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    //reading stream    
                    var ServiceResult = rd.ReadToEnd();
                    //writting stream result on console    
                    Console.WriteLine(ServiceResult);                  
                    return ServiceResult;
                }
            }

        }

        public  void setCustomerWithAfm(string afm, Πελάτης customer)
        {
            try
            { 
                xml = InvokeService(afm);
            }
            catch (Exception exc)
            {
                Console.WriteLine("{0} --- Exeption In Call AFM inTransaction Caught!!!---", exc);
                return;
            }           
            customer.ΑΦΜ = getInfoFromXML("afm").Trim();
            customer.Επωνυμία = getInfoFromXML("onomasia");
            customer.ΔΟΥ = GetΔΟΥ(getInfoFromXML("doy"), (UnitOfWork)customer.Session);
            customer.ΔιευθύνσειςΠελάτη.Add(GetAddress(getInfoFromXML("postal_zip_code"), (UnitOfWork)customer.Session));
            error = getInfoFromXML("error_descr");
        }

        private ΔιευθύνσειςΠελάτη GetAddress(string onomatk, UnitOfWork session)
        {
            ΔιευθύνσειςΠελάτη διευθυνση = new ΔιευθύνσειςΠελάτη(session);
            διευθυνση.SmartOid = Guid.NewGuid();
            διευθυνση.CanUpload = true;
            διευθυνση.Οδός = getInfoFromXML("postal_address");
            διευθυνση.Αριθμός = getInfoFromXML("postal_address_no");
            var tk = new XPCollection<ΤαχυδρομικόςΚωδικός>(session);
            foreach (var i in tk)
            {
                if (i.Ονοματκ == onomatk)
                {
                    διευθυνση.ΤΚ = i;
                    διευθυνση.Πόλη = i.Πόλη;
                    διευθυνση.Περιοχή = i.Περιοχή;
                }
            }
           // διευθυνση.ΤΚ = getInfoFromXML("postal_zip_code");
            return διευθυνση;
        }

        public  string getInfoFromXML(string cvar)
        {

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);
            XmlNodeList nodeList = xmldoc.GetElementsByTagName(cvar);
            string info = string.Empty;

            foreach (XmlNode node in nodeList)
            {
                info = node.InnerText;
            }

            return info;
        }
        public ΔΟΥ GetΔΟΥ(string kodikosDOY, UnitOfWork uow)
        {

                ΔΟΥ doy = null;
                var d = new XPCollection<ΔΟΥ>(uow);

                foreach (var i in d)
                {
                    if(i.Κωδικός == kodikosDOY)
                    {
                       doy = i;
                    }
                }
                return doy;           
                                  
        }
    }
}

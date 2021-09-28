using DevExpress.Xpo;
using System;

namespace SmartMobileProject.Models
{
    public class Εργασία : XPObject
    {
        public Εργασία() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Εργασία(Session session) : base(session)
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
        public string Χαρακτηρισμός
        {
            get { return χαρακτηρισμός; }
            set { SetPropertyValue(nameof(Χαρακτηρισμός), ref χαρακτηρισμός, value); }
        }
        string χαρακτηρισμός;

        [Association]
        public Πωλητής Πωλητής
        {
            get { return πωλητής; }
            set { SetPropertyValue(nameof(Πωλητής), ref πωλητής, value); }
        }
        Πωλητής πωλητής;
        public Πελάτης Πελάτης
        {
            get { return πελάτης; }
            set { SetPropertyValue(nameof(Πελάτης), ref πελάτης, value); }
        }
        Πελάτης πελάτης;
        public DateTime ΗμνίαΕναρξης
        {
            get { return ημνίαεναρξης; }
            set { SetPropertyValue(nameof(ΗμνίαΕναρξης), ref ημνίαεναρξης, value); }
        }
        DateTime ημνίαεναρξης;
        public DateTime ΗμνίαΛηξης
        {
            get { return ημνίαληξης; }
            set { SetPropertyValue(nameof(ΗμνίαΛηξης), ref ημνίαληξης, value); }
        }
        DateTime ημνίαληξης;
        public bool Κατάσταση
        {
            get { return κατάσταση; }
            set { SetPropertyValue(nameof(Κατάσταση), ref κατάσταση, value); }
        }
        bool κατάσταση;
        public string Περιγραφή
        {
            get { return περιγραφή; }
            set { SetPropertyValue(nameof(Περιγραφή), ref περιγραφή, value); }
        }
        string περιγραφή;
        public double ΓεωγραφικόΜήκος
        {
            get { return γεωγραφικόμήκος; }
            set { SetPropertyValue(nameof(ΓεωγραφικόΜήκος), ref γεωγραφικόμήκος, value); }
        }
        double γεωγραφικόμήκος;

        public double ΓεωγραφικόΠλάτος
        {
            get { return γεωγραφικόπλάτος; }
            set { SetPropertyValue(nameof(ΓεωγραφικόΠλάτος), ref γεωγραφικόπλάτος, value); }
        }
        double γεωγραφικόπλάτος;
        [Association]
        public XPCollection<Ενέργεια> Ενέργειες
        {
            get { return GetCollection<Ενέργεια>(nameof(Ενέργειες)); }
        }
        public bool IsUploaded
        {
            get { return canUpload; }
            set { SetPropertyValue(nameof(IsUploaded), ref canUpload, value); }
        }
        bool canUpload;

        [NonPersistent]
        public string DisplayEndTime
        {
            get 
            {
                if (Κατάσταση)
                {
                    displayEndTime = "Ολοκληρώθηκε :" + ΗμνίαΛηξης;
                }
                return displayEndTime; 
            }
            set
            {
                SetPropertyValue(nameof(DisplayEndTime), ref displayEndTime, value);
            }
        }
        string displayEndTime;
        [NonPersistent]
        string politis
        {
            get
            {
                if (Πωλητής != null)
                    return Πωλητής.SmartOid.ToString();
                return string.Empty;
            }
        }
        [NonPersistent]
        string pelatis
        {
            get
            {
                if (Πελάτης != null)
                    return Πελάτης.SmartOid.ToString();
                return string.Empty;
            }
        }
        public string ToJson()
        {
            string json = string.Empty;
            json = "{ \"Oid\":\"" + smartOid +
                "\",\"Πελάτης\":\"" + pelatis +
                "\",\"Πωλητής\":\"" + politis +
                "\",\"ΧαρακτηρισμόςΕργασίας\":\"" + (string.IsNullOrEmpty(Χαρακτηρισμός)?"": Χαρακτηρισμός) +
                "\",\"Περιγραφή\":\"" + (string.IsNullOrEmpty(Περιγραφή) ? "" : Περιγραφή) +
                "\",\"ΗμνίαΕναρξης\":\"" + ΗμνίαΕναρξης.ToString("dd/MM/yyyy HH:mm:ss") +
                "\",\"ΗμνίαΛηξης\":\"" + ΗμνίαΛηξης.ToString("dd/MM/yyyy HH:mm:ss") +
                "\",\"ΓεωγραφικόΜήκος\":\"" + ΓεωγραφικόΜήκος +
                "\",\"ΓεωγραφικόΠλάτος\":\"" + ΓεωγραφικόΠλάτος +
                "\"}";
            return json;
        }
    }

}
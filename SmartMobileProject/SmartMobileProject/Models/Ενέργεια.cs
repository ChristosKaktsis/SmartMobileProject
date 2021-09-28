using DevExpress.Xpo;
using System;
using Xamarin.Essentials;

namespace SmartMobileProject.Models
{
    public class Ενέργεια : XPObject
    {
        public Ενέργεια() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Ενέργεια(Session session) : base(session)
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
        public string Τύπος
        {
            get { return τύπος; }
            set { SetPropertyValue(nameof(Τύπος), ref τύπος, value); }
        }
        string τύπος;
        public string Περιγραφή
        {
            get { return περιγραφή; }
            set { SetPropertyValue(nameof(Περιγραφή), ref περιγραφή, value); }
        }
        string περιγραφή;

        public string Φωτογραφία
        {
            get { return φωτογραφία; }
            set { SetPropertyValue(nameof(Φωτογραφία), ref φωτογραφία, value); }
        }
        string φωτογραφία;

        public string Image
        {
            get { return image; }
            set { SetPropertyValue(nameof(Image), ref image, value); }
        }
        string image;
        public int Αριθμός
        {
            get { return αριθμός; }
            set { SetPropertyValue(nameof(Αριθμός), ref αριθμός, value); }
        }
        int αριθμός;
        public int Num
        {
            get { return num; }
            set { SetPropertyValue(nameof(Num), ref num, value); }
        }
        int num;
        public bool Option
        {
            get { return option; }
            set { SetPropertyValue(nameof(Option), ref option, value); }
        }
        bool option;

        public string Επιλογή
        {
            get { return επιλογή; }
            set { SetPropertyValue(nameof(Επιλογή), ref επιλογή, value); }
        }
        string επιλογή;
        public DateTime Ημερ
        {
            get { return ημερ; }
            set { SetPropertyValue(nameof(Ημερ), ref ημερ, value); }
        }
        DateTime ημερ;
        public bool IsUploaded
        {
            get { return canUpload; }
            set { SetPropertyValue(nameof(IsUploaded), ref canUpload, value); }
        }
        bool canUpload;

        [NonPersistent]
        public string DisplayValue
        {
            get
            {
                string val = "";
               switch(Num)
                {
                    case 0:
                        val = περιγραφή;
                        break;
                    case 1:
                        val = αριθμός.ToString();
                        break;
                    case 2:
                        val = ημερ.ToString();
                        break;
                    case 3:
                        val = επιλογή;
                        break;
                    case 4:
                        val = option ? "Ναι" : "Οχι";
                        break;
                }
                return val;
            }
        }

        [Association]
        public Εργασία Εργασία
        {
            get { return εργασία; }
            set { SetPropertyValue(nameof(Εργασία), ref εργασία, value); }
        }
        Εργασία εργασία;
        public string ToJson()
        {
            string json = string.Empty;
            json = "{ \"Oid\":\"" + smartOid +
                "\",\"Εργασία\":\"" + Εργασία.SmartOid +
                 "\",\"ΤύποςΕνέργειας\":\"" + (string.IsNullOrEmpty(Τύπος) ? "" : Τύπος) +
                "\",\"Περιγραφή\":\"" + (string.IsNullOrEmpty(Περιγραφή) ? "" : Περιγραφή) +
                "\",\"Αριθμός\":\"" + (string.IsNullOrEmpty(Αριθμός.ToString()) ? "" : Αριθμός.ToString()) +
                "\",\"Επιλογή\":\"" + (string.IsNullOrEmpty(Επιλογή) ? "" : Επιλογή) +
                "\",\"Ημνία\":\"" + (string.IsNullOrEmpty(Ημερ.ToString()) ? "" : Ημερ.ToString("dd/MM/yyyy HH:mm:ss")) +
                "\",\"Option\":\"" + (string.IsNullOrEmpty(Option.ToString()) ? "" : Option.ToString()) +
                "\"}";
            return json;
        }
    }

}
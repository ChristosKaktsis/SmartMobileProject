using DevExpress.Xpo;
using SmartMobileProject.Core;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class SettingsStaticSourceViewModel : BaseViewModel
    {
        UnitOfWork uow;
       
        public XPCollection<ΔΟΥ> ΔΟΥ { get; set; }
        public XPCollection<ΦΠΑ> FPA { get; set; }
        ΣτοιχείαΕταιρίας εταιρία;
        public  ΣτοιχείαΕταιρίας Εταιρία
        {
            get
            {
                return εταιρία;
            }
            set
            {
                SetProperty(ref εταιρία, value);
            }
        }
        private ΤαχυδρομικόςΚωδικός tk;
        public ΤαχυδρομικόςΚωδικός Tk
        {
            get
            {
                return tk;
            }
            set
            {
                SetProperty(ref tk, value);
                Εταιρία.ΤΚ = value;
                if (value != null)
                {
                    Εταιρία.Περιοχή = value.Περιοχή;
                    Εταιρία.Πόλη = value.Πόλη;
                }   
            }
        }
        public XPCollection<ΤαχυδρομικόςΚωδικός> TK { get; set; }
        public XPCollection<Πόλη> Poli { get; set; }
        public enum KatFPA { Κανονικό=0 ,Εξαίρεση=1 ,Μειωμένο=2 ,ΤρίτωνΧωρών=3 , ΕντόςΕΕ=4}
        string κατΦΠΑ;
        public string ΚατΦΠΑ
        {
            get { return κατΦΠΑ; }
        }
        public SettingsStaticSourceViewModel()
        {  
            SetAtt();
            if (OnlineMode)
                Check();
            else
                SimpleCheck();

            Αποθήκευση = new Command(Save);
        }

        

        private void SetAtt()
        {
            uow = new UnitOfWork();
            Εταιρία = new ΣτοιχείαΕταιρίας(uow);
            ΔΟΥ = new XPCollection<ΔΟΥ>(uow);
            FPA = new XPCollection<ΦΠΑ>(uow);
            TK = new XPCollection<ΤαχυδρομικόςΚωδικός>(uow);
            Poli = new XPCollection<Πόλη>(uow);

        }

        async void Check()
        {
            await XpoHelper.CreateSTOIXEIAETAIRIASData();
            await Task.Run(() => 
            {               
                var list = uow.Query<ΣτοιχείαΕταιρίας>();
                if (list.Any())
                {
                    Εταιρία = list.First();
                    Tk = Εταιρία.ΤΚ;
                    κατΦΠΑ = ((KatFPA)Εταιρία.ΚατηγορίαΦΠΑ).ToString();
                }
            });
            
        }
        private void SimpleCheck()
        {
            var list = uow.Query<ΣτοιχείαΕταιρίας>();
            if (list.Any())
            {
                Εταιρία = list.First();
                Tk = Εταιρία.ΤΚ;
                κατΦΠΑ = ((KatFPA)Εταιρία.ΚατηγορίαΦΠΑ).ToString();
            }
        }
        private  void Save(object obj)
        {
            if (uow.InTransaction)
            {
                uow.CommitChanges();
            }
        }
        public ICommand Αποθήκευση { set; get; }

    }
}

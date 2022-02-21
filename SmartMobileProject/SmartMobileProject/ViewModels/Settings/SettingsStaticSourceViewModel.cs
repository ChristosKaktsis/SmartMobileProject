using DevExpress.Xpo;
using SmartMobileProject.Core;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                if (value == null)
                    return;
                Επωνυμία = value.Επωνυμία;
                Tk = value.ΤΚ;
                ΔΟΥ1 = value.ΔΟΥ;
                ΔικτυακόςΤόπος = value.ΔικτυακόςΤόπος;
                ΑΦΜ = value.ΑΦΜ;
                κατΦΠΑ = ((KatFPA)value.ΚατηγορίαΦΠΑ).ToString();
                Οδός = value.Οδός;
                Αριθμός = value.Αριθμός;
                Τηλέφωνο = value.Τηλέφωνο;
                FAX = value.FAX;
                Email = value.Email;
                Username = value.UsernameΥπηρεσίαςΣτοιχείωνΜητρώου;
                Password = value.PasswordΥπηρεσίαςΣτοιχείωνΜητρώου;
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
                //Εταιρία.ΤΚ = value;
                //if (value != null)
                //{
                //    Εταιρία.Περιοχή = value.Περιοχή;
                //    Εταιρία.Πόλη = value.Πόλη;
                //}   
                if (value == null)
                    return;
                Περιοχή = value.Περιοχή;
                ΠΟΛΗ1 = value.Πόλη;
            }
        }
        public string Επωνυμία
        {
            get
            {
                return _Επωνυμία;
            }
            set
            {
                SetProperty(ref _Επωνυμία, value);
            }
        }
        public string ΔικτυακόςΤόπος
        {
            get
            {
                return _ΔικτυακόςΤόπος;
            }
            set
            {
                SetProperty(ref _ΔικτυακόςΤόπος, value);
            }
        }
        public string ΑΦΜ
        {
            get
            {
                return αφμ;
            }
            set
            {
                SetProperty(ref αφμ, value);
            }
        }
        public string Οδός
        {
            get
            {
                return _Οδός;
            }
            set
            {
                SetProperty(ref _Οδός, value);
            }
        }
        public string Αριθμός
        {
            get
            {
                return _Αριθμός;
            }
            set
            {
                SetProperty(ref _Αριθμός, value);
            }
        }
        public string Περιοχή
        {
            get
            {
                return _Περιοχή;
            }
            set
            {
                SetProperty(ref _Περιοχή, value);
            }
        }
        public string Τηλέφωνο
        {
            get
            {
                return _Τηλέφωνο;
            }
            set
            {
                SetProperty(ref _Τηλέφωνο, value);
            }
        }
        public string FAX
        {
            get
            {
                return _FAX;
            }
            set
            {
                SetProperty(ref _FAX, value);
            }
        }
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                SetProperty(ref _Email, value);
            }
        }
        public string Username
        {
            get
            {
                return _Username;
            }
            set
            {
                SetProperty(ref _Username, value);
            }
        }
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                SetProperty(ref _Password, value);
            }
        }
        public ΔΟΥ ΔΟΥ1
        {
            get
            {
                return δου;
            }
            set
            {
                SetProperty(ref δου, value);
            }
        }
        public ΦΠΑ ΦΠΑ1
        {
            get
            {
                return φπα;
            }
            set
            {
                SetProperty(ref φπα, value);
            }
        }
        public Πόλη ΠΟΛΗ1
        {
            get
            {
                return πολη;
            }
            set
            {
                SetProperty(ref πολη, value);
            }
        }
        //public XPCollection<ΤαχυδρομικόςΚωδικός> TK { get; set; }
        public ObservableCollection<ΤαχυδρομικόςΚωδικός> TK { get; set; }
        //public XPCollection<Πόλη> Poli { get; set; }
        public ObservableCollection<Πόλη> Poli { get; set; }
        public ObservableCollection<ΔΟΥ> ΔΟΥ { get; set; }
        public ObservableCollection<ΦΠΑ> FPA { get; set; }
        public enum KatFPA { Κανονικό=0 ,Εξαίρεση=1 ,Μειωμένο=2 ,ΤρίτωνΧωρών=3 , ΕντόςΕΕ=4}
        string κατΦΠΑ;
        private ΔΟΥ δου;
        private ΦΠΑ φπα;
        private Πόλη πολη;
        private string _Επωνυμία;
        private string _ΔικτυακόςΤόπος;
        private string αφμ;
        private string _Οδός;
        private string _Αριθμός;
        private string _Περιοχή;
        private string _Τηλέφωνο;
        private string _FAX;
        private string _Password;
        private string _Email;
        private string _Username;

        public string ΚατΦΠΑ
        {
            get { return κατΦΠΑ; }
        }
        public SettingsStaticSourceViewModel()
        {
            //SetAtt();
            //if (OnlineMode)
            //    Check();
            //else
            //    SimpleCheck();
            ΔΟΥ = new ObservableCollection<ΔΟΥ>();
            FPA = new ObservableCollection<ΦΠΑ>();
            TK = new ObservableCollection<ΤαχυδρομικόςΚωδικός>();
            Poli = new ObservableCollection<Πόλη>();
            LoadCompanyCommand = new Command(async () => await OnLoadCompany());
            Αποθήκευση = new Command(Save);
        }
        private async Task OnLoadCompany()
        {
            try
            {
                IsBusy = true;
                ΔΟΥ.Clear();
                FPA.Clear();
                TK.Clear();
                Poli.Clear();
                if (OnlineMode)
                    await XpoHelper.CreateSTOIXEIAETAIRIASData();
                using (UnitOfWork uow = new UnitOfWork())
                {
                    var doyitems = uow.Query<ΔΟΥ>();
                    var fpaitems = uow.Query<ΦΠΑ>();
                    var tkitems = uow.Query<ΤαχυδρομικόςΚωδικός>();
                    var poliitems = uow.Query<Πόλη>();
                    foreach (var item in doyitems)
                        ΔΟΥ.Add(item);
                    foreach (var item in fpaitems)
                        FPA.Add(item);
                    foreach (var item in tkitems)
                        TK.Add(item);
                    foreach (var item in poliitems)
                        Poli.Add(item);
                    
                    Εταιρία = uow.Query<ΣτοιχείαΕταιρίας>().FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert("Σφάλμα",
                     "Κάτι πήγε λάθος στο φόρτομα των δεδομένων εταιρείας", "Εντάξει");
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }
        //private void SetAtt()
        //{
        //    uow = new UnitOfWork();
        //    Εταιρία = new ΣτοιχείαΕταιρίας(uow);
        //    ΔΟΥ = new XPCollection<ΔΟΥ>(uow);
        //    FPA = new XPCollection<ΦΠΑ>(uow);
        //    TK = new XPCollection<ΤαχυδρομικόςΚωδικός>(uow);
        //    Poli = new XPCollection<Πόλη>(uow);

        //}
        //async void Check()
        //{
        //    await XpoHelper.CreateSTOIXEIAETAIRIASData();
        //    await Task.Run(() => 
        //    {               
        //        var list = uow.Query<ΣτοιχείαΕταιρίας>();
        //        if (list.Any())
        //        {
        //            Εταιρία = list.First();
        //            Tk = Εταιρία.ΤΚ;
        //            κατΦΠΑ = ((KatFPA)Εταιρία.ΚατηγορίαΦΠΑ).ToString();
        //        }
        //    });
            
        //}
        //private void SimpleCheck()
        //{
        //    var list = uow.Query<ΣτοιχείαΕταιρίας>();
        //    if (list.Any())
        //    {
        //        Εταιρία = list.First();
        //        Tk = Εταιρία.ΤΚ;
        //        κατΦΠΑ = ((KatFPA)Εταιρία.ΚατηγορίαΦΠΑ).ToString();
        //    }
        //}
        private async void Save(object obj)
        {
            //if (uow.InTransaction)
            //{
            //    uow.CommitChanges();
            //    await Application.Current.MainPage.DisplayAlert("Αποθήκευση",
            //         "Οι Αλλαγές Αποθηκεύτηκαν", "Εντάξει");
            //}
            try
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    ΣτοιχείαΕταιρίας νεαΕταιρία;
                    if (Εταιρία == null)
                        νεαΕταιρία = new ΣτοιχείαΕταιρίας(uow);
                    else
                        νεαΕταιρία = uow.Query<ΣτοιχείαΕταιρίας>().Where(x => x.Oid == Εταιρία.Oid).FirstOrDefault();
                    νεαΕταιρία.Επωνυμία = Επωνυμία;
                    νεαΕταιρία.ΤΚ = uow.Query<ΤαχυδρομικόςΚωδικός>().Where(x => x.Oid == Tk.Oid).FirstOrDefault();
                    νεαΕταιρία.ΔΟΥ = uow.Query<ΔΟΥ>().Where(x => x.Oid == ΔΟΥ1.Oid).FirstOrDefault();
                    νεαΕταιρία.ΔικτυακόςΤόπος = ΔικτυακόςΤόπος;
                    νεαΕταιρία.ΑΦΜ = ΑΦΜ;
                    νεαΕταιρία.Οδός = Οδός;
                    νεαΕταιρία.Αριθμός = Αριθμός;
                    νεαΕταιρία.Τηλέφωνο = Τηλέφωνο;
                    νεαΕταιρία.FAX = FAX;
                    νεαΕταιρία.Email = Email;
                    νεαΕταιρία.UsernameΥπηρεσίαςΣτοιχείωνΜητρώου = Username;
                    νεαΕταιρία.PasswordΥπηρεσίαςΣτοιχείωνΜητρώου = Password;
                    uow.Save(νεαΕταιρία);
                    uow.CommitTransaction();
                }

                await Application.Current.MainPage.DisplayAlert("Αποθήκευση",
                         "Οι Αλλαγές Αποθηκεύτηκαν", "Εντάξει");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert("Σφάλμα",
                         "Οι Αλλαγές Δεν αποθηκευτηκαν", "Εντάξει");
            }
            
        }
        public ICommand LoadCompanyCommand { set; get; }
        public ICommand Αποθήκευση { set; get; }

    }
}

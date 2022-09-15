using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΣειρέςΠαραστατικώνΠωλήσεωνViewModel : BaseViewModel
    {
        public ΣειρέςΠαραστατικώνΠωλήσεωνViewModel()
        {
            Αποθήκευση = new Command(Save);
        }
        ΣειρέςΠαραστατικώνΠωλήσεων σειρά;
        public ΣειρέςΠαραστατικώνΠωλήσεων Σειρά
        {
            get
            {
                return σειρά;
            }
            set
            {
                SetProperty(ref σειρά, value);
            }
        }
        string πρόθεμα;
        public string Πρόθεμα
        {
            get
            {
                return πρόθεμα;
            }
            set
            {
                SetProperty(ref πρόθεμα, value);
            }
        }
        string περιγραφή;
        public string Περιγραφή
        {
            get
            {
                return περιγραφή;
            }
            set
            {
                SetProperty(ref περιγραφή, value);
            }
        }
        private int μετρητής;
        public int Μετρητής
        {
            get { return μετρητής; }
            set { SetProperty(ref μετρητής, value); }
        }
        string printType;
        public string PrintType
        {
            get
            {
                return printType;
            }
            set
            {
                SetProperty(ref printType, value);
            }
        }
        private async void Save(object obj)
        {
            if (string.IsNullOrEmpty(Πρόθεμα) || string.IsNullOrEmpty(Περιγραφή))
                return;
            var currentP = ((AppShell)Application.Current.MainPage).πωλητής;
            if (currentP == null)
            {
                Console.WriteLine("Save Seires Politis Is Null !!!!");
                return;
            }
            using (UnitOfWork uow = new UnitOfWork())
            {
                ΣειρέςΠαραστατικώνΠωλήσεων σειρές;
                if (Σειρά == null)
                {
                    σειρές = new ΣειρέςΠαραστατικώνΠωλήσεων(uow);
                    σειρές.SmartOid = Guid.NewGuid();
                    σειρές.IDΠωλητή = currentP.SmartOid;
                }
                else
                {
                    σειρές = uow.GetObjectByKey<ΣειρέςΠαραστατικώνΠωλήσεων>(Σειρά.Oid);
                }
                
                σειρές.Σειρά = Πρόθεμα;
                σειρές.Περιγραφή = Περιγραφή;
                σειρές.Counter = Μετρητής;
                σειρές.PrintType = PrintType;
                await uow.CommitChangesAsync();
            }
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        public ICommand Αποθήκευση { set; get; }
    }
}

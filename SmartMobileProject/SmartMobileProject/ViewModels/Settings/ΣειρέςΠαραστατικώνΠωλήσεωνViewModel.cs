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

            using(UnitOfWork uow = new UnitOfWork())
            {
                ΣειρέςΠαραστατικώνΠωλήσεων σειρές;
                if (Σειρά == null)
                {
                    σειρές = new ΣειρέςΠαραστατικώνΠωλήσεων(uow);
                    σειρές.SmartOid = Guid.NewGuid();
                }
                else
                {
                    σειρές = uow.GetObjectByKey<ΣειρέςΠαραστατικώνΠωλήσεων>(Σειρά.Oid);
                }
                
                σειρές.Σειρά = Πρόθεμα;
                σειρές.Περιγραφή = Περιγραφή;
                σειρές.PrintType = PrintType;
                await uow.CommitChangesAsync();
            }
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        public ICommand Αποθήκευση { set; get; }
    }
}

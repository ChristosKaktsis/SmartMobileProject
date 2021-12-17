using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΣειρέςΠαραστατικώνΕισπράξεωνViewModel : BaseViewModel
    {
        public ΣειρέςΠαραστατικώνΕισπράξεωνViewModel()
        {
            Αποθήκευση = new Command(Save);
        }
        ΣειρέςΠαραστατικώνΕισπράξεων σειρά;
        public ΣειρέςΠαραστατικώνΕισπράξεων Σειρά
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
        private async void Save(object obj)
        {
            if (string.IsNullOrEmpty(Πρόθεμα) || string.IsNullOrEmpty(Περιγραφή))
                return;

            using(UnitOfWork uow = new UnitOfWork())
            {
                ΣειρέςΠαραστατικώνΕισπράξεων σειρές; 
                if (Σειρά == null)
                {
                    σειρές = new ΣειρέςΠαραστατικώνΕισπράξεων(uow);
                    σειρές.SmartOid = Guid.NewGuid();
                }
                else
                {
                    σειρές = uow.GetObjectByKey<ΣειρέςΠαραστατικώνΕισπράξεων>(Σειρά.Oid);
                }
                
                σειρές.Σειρά = Πρόθεμα;
                σειρές.Περιγραφή = Περιγραφή;
                await uow.CommitChangesAsync();
            }
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        public ICommand Αποθήκευση { set; get; }
    }
}

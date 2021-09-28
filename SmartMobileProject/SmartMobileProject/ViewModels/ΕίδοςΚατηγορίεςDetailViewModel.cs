using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΕίδοςΚατηγορίεςDetailViewModel : BaseViewModel
    {
        public ΕίδοςΚατηγορίεςDetailViewModel()
        {
            Αποθήκευση = new Command(Save);
        }
        ΚατηγορίαΕίδους οικογένειαΕίδους;
        public ΚατηγορίαΕίδους ΚατηγορίαΕίδους
        {
            get
            {
                return οικογένειαΕίδους;
            }
            set
            {
                SetProperty(ref οικογένειαΕίδους, value);
                Περιγραφή = value.Περιγραφή;
                Κατηγορία = value.Κατηγορία;
            }
        }
        string κατη;
        public string Κατηγορία
        {
            get
            {
                return κατη;
            }
            set
            {
                SetProperty(ref κατη, value);
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
            if (string.IsNullOrEmpty(Περιγραφή))
                return;

            using (UnitOfWork uow = new UnitOfWork())
            {
                ΚατηγορίαΕίδους κατηγορία;
                if (ΚατηγορίαΕίδους == null)
                {
                    κατηγορία = new ΚατηγορίαΕίδους(uow);
                    κατηγορία.SmartOid = Guid.NewGuid();
                }
                else
                {
                    κατηγορία = uow.GetObjectByKey<ΚατηγορίαΕίδους>(ΚατηγορίαΕίδους.Oid);
                }
                κατηγορία.Κατηγορία = Κατηγορία;
                κατηγορία.Περιγραφή = Περιγραφή;
                await uow.CommitChangesAsync();
            }
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        public ICommand Αποθήκευση { set; get; }
    }
}

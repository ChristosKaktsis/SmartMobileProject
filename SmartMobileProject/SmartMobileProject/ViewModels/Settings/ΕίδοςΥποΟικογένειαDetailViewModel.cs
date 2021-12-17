using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΕίδοςΥποΟικογένειαDetailViewModel : BaseViewModel
    {
        public ΕίδοςΥποΟικογένειαDetailViewModel()
        {
            Αποθήκευση = new Command(Save);
        }
        ΥποοικογένειαΕίδους υποοικογένειαΕίδους;
        public ΥποοικογένειαΕίδους ΥποοικογένειαΕίδους
        {
            get
            {
                return υποοικογένειαΕίδους;
            }
            set
            {
                SetProperty(ref υποοικογένειαΕίδους, value);
                Περιγραφή = value.Περιγραφή;
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
                ΥποοικογένειαΕίδους οικογένεια;
                if (ΥποοικογένειαΕίδους == null)
                {
                    οικογένεια = new ΥποοικογένειαΕίδους(uow);
                    οικογένεια.SmartOid = Guid.NewGuid();
                }
                else
                {
                    οικογένεια = uow.GetObjectByKey<ΥποοικογένειαΕίδους>(ΥποοικογένειαΕίδους.Oid);
                }
                οικογένεια.Περιγραφή = Περιγραφή;
                await uow.CommitChangesAsync();
            }
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        public ICommand Αποθήκευση { set; get; }
    }
}

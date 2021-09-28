using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΕίδοςΟικογένειαDetailViewModel : BaseViewModel
    {
        public ΕίδοςΟικογένειαDetailViewModel()
        {
            Αποθήκευση = new Command(Save);
        }
        ΟικογένειαΕίδους οικογένειαΕίδους;
        public ΟικογένειαΕίδους ΟικογένειαΕίδους
        {
            get
            {
                return οικογένειαΕίδους;
            }
            set
            {
                SetProperty(ref οικογένειαΕίδους, value);
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
                ΟικογένειαΕίδους οικογένεια;
                if (ΟικογένειαΕίδους == null)
                {
                    οικογένεια = new ΟικογένειαΕίδους(uow);
                    οικογένεια.SmartOid = Guid.NewGuid();
                }
                else
                {
                    οικογένεια = uow.GetObjectByKey<ΟικογένειαΕίδους>(ΟικογένειαΕίδους.Oid);
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

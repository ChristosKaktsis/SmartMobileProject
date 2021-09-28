using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΕίδοςΟμάδαDetailViewModel : BaseViewModel
    {
        public ΕίδοςΟμάδαDetailViewModel()
        {
            Αποθήκευση = new Command(Save);
        }
        ΟμάδαΕίδους ομάδαΕίδους;
        public ΟμάδαΕίδους ΟμάδαΕίδους
        {
            get
            {
                return ομάδαΕίδους;
            }
            set
            {
                SetProperty(ref ομάδαΕίδους, value);
                Ομάδα = value.Ομάδα;
            }
        }
        string περιγραφή;
        public string Ομάδα
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
            if (string.IsNullOrEmpty(Ομάδα))
                return;

            using (UnitOfWork uow = new UnitOfWork())
            {
                ΟμάδαΕίδους ομάδα;
                if (ΟμάδαΕίδους == null)
                {
                    ομάδα = new ΟμάδαΕίδους(uow);
                    ομάδα.SmartOid = Guid.NewGuid();
                }
                else
                {
                    ομάδα = uow.GetObjectByKey<ΟμάδαΕίδους>(ΟμάδαΕίδους.Oid);
                }
                ομάδα.Ομάδα = Ομάδα;
                await uow.CommitChangesAsync();
            }
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        public ICommand Αποθήκευση { set; get; }
    }
}

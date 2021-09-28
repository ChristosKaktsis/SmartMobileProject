using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΔΟΥDetailViewModel : BaseViewModel
    {
        public ΔΟΥDetailViewModel()
        {
            Αποθήκευση = new Command(Save);
        }
        ΔΟΥ δΟΥ;
        public ΔΟΥ ΔΟΥ
        {
            get
            {
                return δΟΥ;
            }
            set
            {
                SetProperty(ref δΟΥ, value);
                Περιγραφή = value.Περιγραφή;
                Κωδικός = value.Κωδικός;
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
        string κωδικός;
        public string Κωδικός
        {
            get
            {
                return κωδικός;
            }
            set
            {
                SetProperty(ref κωδικός, value);
            }
        }
        private async void Save(object obj)
        {
            if (string.IsNullOrEmpty(Περιγραφή))
                return;

            using (UnitOfWork uow = new UnitOfWork())
            {
                ΔΟΥ δου;
                if (ΔΟΥ == null)
                {
                    δου = new ΔΟΥ(uow);
                    δου.SmartOid = Guid.NewGuid();
                }
                else
                {
                    δου = uow.GetObjectByKey<ΔΟΥ>(ΔΟΥ.Oid);
                }

                δου.Περιγραφή = Περιγραφή;
                δου.Κωδικός = Κωδικός;
                
                await uow.CommitChangesAsync();
            }
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        public ICommand Αποθήκευση { set; get; }
    }
}

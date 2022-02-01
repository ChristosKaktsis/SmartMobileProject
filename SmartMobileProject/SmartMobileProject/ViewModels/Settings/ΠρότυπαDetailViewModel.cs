using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels.Settings
{
    public class ΠρότυπαDetailViewModel : BaseViewModel
    {
        public ΠρότυπαDetailViewModel()
        {
            Αποθήκευση = new Command(Save);
        }
        int oid=-1;
        public int Oid
        {
            get
            {
                return oid;
            }
            set
            {
                SetProperty(ref oid, value);
                SetThisItem(value);
            }
        }

        private void SetThisItem(int value)
        {
            using(UnitOfWork uow = new UnitOfWork())
            {
                Όνομα = uow.GetObjectByKey<Πρότυπα>(value).Περιγραφή;
            }
            
        }
        string όνομα;
        public string Όνομα
        {
            get
            {
                return όνομα;
            }
            set
            {
                SetProperty(ref όνομα, value);
            }
        }
        private async void Save(object obj)
        {
            if (string.IsNullOrEmpty(Όνομα))
                return;

            using (UnitOfWork uow = new UnitOfWork())
            {
                Πρότυπα Πρότυπα; 
                if (Oid == -1)
                {
                    Πρότυπα = new Πρότυπα(uow);

                }
                else
                {
                    Πρότυπα = uow.GetObjectByKey<Πρότυπα>(Oid);
                }

                Πρότυπα.Περιγραφή = Όνομα;

                await uow.CommitChangesAsync();
            }
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        public Command Αποθήκευση { set; get; }
    }
}

using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΤρόποςΠληρωμήςDetailViewModel : BaseViewModel
    {
        private ΤρόποςΠληρωμής _τρόποςΠληρωμής;
        private string όνομα;

        public ICommand Αποθήκευση { set; get; }
        public ΤρόποςΠληρωμήςDetailViewModel()
        {
            Αποθήκευση = new Command(Save);
        }
        public ΤρόποςΠληρωμής ΤρόποςΠληρωμής
        {
            get
            {
                return _τρόποςΠληρωμής;
            }
            set
            {
                SetProperty(ref _τρόποςΠληρωμής, value);
                Όνομα = value.Τρόποςπληρωμής;
            }
        }
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
                ΤρόποςΠληρωμής πόλης;
                if (ΤρόποςΠληρωμής == null)
                {
                    πόλης = new ΤρόποςΠληρωμής(uow);
                    πόλης.SmartOid = Guid.NewGuid();
                }
                else
                {
                    πόλης = uow.GetObjectByKey<ΤρόποςΠληρωμής>(ΤρόποςΠληρωμής.Oid);
                }

                πόλης.Τρόποςπληρωμής = Όνομα;

                await uow.CommitChangesAsync();
            }
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}

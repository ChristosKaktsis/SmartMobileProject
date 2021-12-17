using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΤρόποςΑποστολήςDetailViewModel : BaseViewModel
    {
        private ΤρόποςΑποστολής _τρόποςΑποστολής;
        private string όνομα;

        public ICommand Αποθήκευση { set; get; }
        public ΤρόποςΑποστολήςDetailViewModel()
        {
            Αποθήκευση = new Command(Save);
        }
        public ΤρόποςΑποστολής ΤρόποςΑποστολής
        {
            get
            {
                return _τρόποςΑποστολής;
            }
            set
            {
                SetProperty(ref _τρόποςΑποστολής, value);
                Όνομα = value.Τρόποςαποστολής;
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
                ΤρόποςΑποστολής πόλης;
                if (ΤρόποςΑποστολής == null)
                {
                    πόλης = new ΤρόποςΑποστολής(uow);
                    πόλης.SmartOid = Guid.NewGuid();
                }
                else
                {
                    πόλης = uow.GetObjectByKey<ΤρόποςΑποστολής>(ΤρόποςΑποστολής.Oid);
                }

                πόλης.Τρόποςαποστολής = Όνομα;

                await uow.CommitChangesAsync();
            }
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}

using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΠόληDetailViewModel : BaseViewModel
    {
        public ΠόληDetailViewModel()
        {
            Αποθήκευση = new Command(Save);
        }
        Πόλη πόλη;
        public Πόλη Πόλη
        {
            get
            {
                return πόλη;
            }
            set
            {
                SetProperty(ref πόλη, value);
                Όνομα = value.ΟνομαΠόλης;
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
                Πόλη πόλης;
                if (Πόλη == null)
                {
                    πόλης = new Πόλη(uow);
                    πόλης.SmartOid = Guid.NewGuid();
                }
                else
                {
                    πόλης = uow.GetObjectByKey<Πόλη>(Πόλη.Oid);
                }

                πόλης.ΟνομαΠόλης = Όνομα;
                
                await uow.CommitChangesAsync();
            }
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        public ICommand Αποθήκευση { set; get; }
    }
}

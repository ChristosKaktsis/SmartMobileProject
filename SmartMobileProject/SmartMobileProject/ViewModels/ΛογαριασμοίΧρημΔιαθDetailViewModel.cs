using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΛογαριασμοίΧρημΔιαθDetailViewModel : BaseViewModel
    {
        public ΛογαριασμοίΧρημΔιαθDetailViewModel()
        {
            Αποθήκευση = new Command(Save);
        }
        ΛογαριασμοίΧρηματικώνΔιαθέσιμων λογαρισαμοί;
        public ΛογαριασμοίΧρηματικώνΔιαθέσιμων Λογαρισαμοί
        {
            get
            {
                return λογαρισαμοί;
            }
            set
            {
                SetProperty(ref λογαρισαμοί, value);
                Όνομα = value.Λογαριασμός;
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
                ΛογαριασμοίΧρηματικώνΔιαθέσιμων λογαριασμος;
                if (Λογαρισαμοί == null)
                {
                    λογαριασμος = new ΛογαριασμοίΧρηματικώνΔιαθέσιμων(uow);
                    λογαριασμος.SmartOid = Guid.NewGuid();
                }
                else
                {
                    λογαριασμος = uow.GetObjectByKey<ΛογαριασμοίΧρηματικώνΔιαθέσιμων>(Λογαρισαμοί.Oid);
                }

                λογαριασμος.Λογαριασμός = Όνομα;
                
                await uow.CommitChangesAsync();
            }
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        public ICommand Αποθήκευση { set; get; }
    }
}

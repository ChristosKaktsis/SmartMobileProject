using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels.Settings
{
    public class ΜεταφορικόDetailViewModel : BaseViewModel
    {
        public ΜεταφορικόDetailViewModel()
        {
            Αποθήκευση = new Command(Save);
        }
        ΜεταφορικόΜέσο μμ;
        public ΜεταφορικόΜέσο ΜΜ
        {
            get
            {
                return μμ;
            }
            set
            {
                SetProperty(ref μμ, value);
                ΑριθμόςΚυκλοφορίας = value.ΑριθμόςΚυκλοφορίας;
            }
        }
        string αριθμόςΚυκλοφορίας;
        public string ΑριθμόςΚυκλοφορίας
        {
            get
            {
                return αριθμόςΚυκλοφορίας;
            }
            set
            {
                SetProperty(ref αριθμόςΚυκλοφορίας, value);
            }
        }
        private async void Save(object obj)
        {
            if (string.IsNullOrEmpty(ΑριθμόςΚυκλοφορίας))
                return;

            using (UnitOfWork uow = new UnitOfWork())
            {
                ΜεταφορικόΜέσο μέσο;
                if (ΜΜ == null)
                {
                    μέσο = new ΜεταφορικόΜέσο(uow);
                    μέσο.SmartOid = Guid.NewGuid();
                }
                else
                {
                    μέσο = uow.GetObjectByKey<ΜεταφορικόΜέσο>(ΜΜ.Oid);
                }

                μέσο.ΑριθμόςΚυκλοφορίας = ΑριθμόςΚυκλοφορίας;

                await uow.CommitChangesAsync();
            }
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        public Command Αποθήκευση { set; get; }
    }
}

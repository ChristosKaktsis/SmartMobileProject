using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ΠωλητήςViewModel : BaseViewModel
    {
        UnitOfWork uow = ((App)Application.Current).uow;
        Πωλητής πωλητής;
        public Πωλητής Πωλητής
        {
            get
            {
                return πωλητής;
            }
            set
            {
                SetProperty(ref πωλητής, value);
            }
        }
        public ΠωλητήςViewModel()
        {
            Πωλητής = ((AppShell)Application.Current.MainPage).πωλητής;
            Preferences.Set("Πωλητής", Πωλητής.Oid.ToString());
            
            Αποθήκευση = new Command(Save);
        }
        private void Save(object obj)
        {
            if (!IsTrialOn)
                return;
            if (uow.InTransaction)
            {
                uow.CommitChanges();
            }
        }   
        public ICommand Αποθήκευση { set; get; }
    }
}

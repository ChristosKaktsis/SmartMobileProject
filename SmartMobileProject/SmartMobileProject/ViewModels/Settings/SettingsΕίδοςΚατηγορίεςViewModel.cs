using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class SettingsΕίδοςΚατηγορίεςViewModel : BaseViewModel
    {
        UnitOfWork uow;

        public SettingsΕίδοςΚατηγορίεςViewModel()
        {
            uow = new UnitOfWork();
            ΚατηγορίαΕίδους = new XPCollection<ΚατηγορίαΕίδους>(uow);
            Προσθήκη = new Command(Create);
        }

        public XPCollection<ΚατηγορίαΕίδους> ΚατηγορίαΕίδους { get; set; }
        private async void Create(object obj)
        {
            await Shell.Current.Navigation.PushAsync(new ΕίδοςΚατηγορίεςDetailViewPage(null));
        }
        public void Save()
        {
            ΚατηγορίαΕίδους.Reload();
        }
        public ICommand Προσθήκη { set; get; }
    }
}

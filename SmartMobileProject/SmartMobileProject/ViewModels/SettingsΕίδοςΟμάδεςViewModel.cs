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
    class SettingsΕίδοςΟμάδεςViewModel : BaseViewModel
    {
        UnitOfWork uow;

        public SettingsΕίδοςΟμάδεςViewModel()
        {
            uow = new UnitOfWork();
            ΟμάδαΕίδους = new XPCollection<ΟμάδαΕίδους>(uow);
            Προσθήκη = new Command(Create);
        }

        public XPCollection<ΟμάδαΕίδους> ΟμάδαΕίδους { get; set; }
        private async void Create(object obj)
        {
            await Shell.Current.Navigation.PushAsync(new ΕίδοςΟμάδαDetailViewPage(null));
        }
        public void Save()
        {
            ΟμάδαΕίδους.Reload();
        }
        public ICommand Προσθήκη { set; get; }
    }
}

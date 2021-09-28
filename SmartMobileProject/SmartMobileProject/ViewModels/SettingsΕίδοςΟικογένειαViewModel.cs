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
    class SettingsΕίδοςΟικογένειαViewModel : BaseViewModel
    {
        UnitOfWork uow;

        public SettingsΕίδοςΟικογένειαViewModel()
        {
            uow = new UnitOfWork();
            ΟικογένειαΕίδους = new XPCollection<ΟικογένειαΕίδους>(uow);
            Προσθήκη = new Command(Create);
        }

        public XPCollection<ΟικογένειαΕίδους> ΟικογένειαΕίδους { get; set; }
        private async void Create(object obj)
        {
            await Shell.Current.Navigation.PushAsync(new ΕίδοςΟικογένειαDetailViewPage(null));
        }
        public void Save()
        {
            ΟικογένειαΕίδους.Reload();
        }
        public ICommand Προσθήκη { set; get; }
    }
}

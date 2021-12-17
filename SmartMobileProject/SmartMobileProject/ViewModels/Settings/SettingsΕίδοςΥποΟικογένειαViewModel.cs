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
    class SettingsΕίδοςΥποΟικογένειαViewModel : BaseViewModel
    {
        UnitOfWork uow;

        public SettingsΕίδοςΥποΟικογένειαViewModel()
        {
            uow = new UnitOfWork();
            ΥποΟικογένειαΕίδους = new XPCollection<ΥποοικογένειαΕίδους>(uow);
            Προσθήκη = new Command(Create);
        }

        public XPCollection<ΥποοικογένειαΕίδους> ΥποΟικογένειαΕίδους { get; set; }
        private async void Create(object obj)
        {
            await Shell.Current.Navigation.PushAsync(new ΕίδοςΥποοικογένειαDetailViewPage(null));
        }
        public void Save()
        {
            ΥποΟικογένειαΕίδους.Reload();
        }
        public ICommand Προσθήκη { set; get; }
    }
}

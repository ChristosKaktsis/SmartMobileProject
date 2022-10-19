using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels.Settings
{
    public class SettingsΜΜViewModel : BaseViewModel
    {
        UnitOfWork uow;

        public SettingsΜΜViewModel()
        {
            uow = new UnitOfWork();
            Μεταφορικό = new XPCollection<ΜεταφορικόΜέσο>(uow);
            Προσθήκη = new Command(Create);
        }
        public XPCollection<ΜεταφορικόΜέσο> Μεταφορικό { get; set; }
        private async void Create(object obj)
        {
            await Shell.Current.Navigation.PushAsync(new ΜεταφορικόDetailViewPage(null));
        }
        public void Save()
        {
            Μεταφορικό.Reload();
        }
        public Command Προσθήκη { set; get; }
    }
}

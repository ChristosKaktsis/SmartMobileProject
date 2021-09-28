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
    class SettingsΠόληViewModel : BaseViewModel
    {
        UnitOfWork uow;

        public SettingsΠόληViewModel()
        {
            uow = new UnitOfWork();
            Πόλη = new XPCollection<Πόλη>(uow);
            Προσθήκη = new Command(Create);
        }

        public XPCollection<Πόλη> Πόλη { get; set; }
        private async void Create(object obj)
        {
            await Shell.Current.Navigation.PushAsync(new ΠόληςDetailViewPage(null));
        }
        public void Save()
        {
            Πόλη.Reload();
        }
        public ICommand Προσθήκη { set; get; }
    }
}

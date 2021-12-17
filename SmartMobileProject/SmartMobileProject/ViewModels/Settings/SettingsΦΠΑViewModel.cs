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
    class SettingsΦΠΑViewModel : BaseViewModel
    {
        UnitOfWork uow;

        public SettingsΦΠΑViewModel()
        {
            uow = new UnitOfWork();
            Πόλη = new XPCollection<ΦΠΑ>(uow);
            Προσθήκη = new Command(Create);
        }

        public XPCollection<ΦΠΑ> Πόλη { get; set; }
        private async void Create(object obj)
        {
            await Shell.Current.Navigation.PushAsync(new ΦΠΑDetailViewPage(null));
        }
        public void Save()
        {
            Πόλη.Reload();
        }
        public ICommand Προσθήκη { set; get; }
    }
}

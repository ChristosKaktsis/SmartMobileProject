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
    class SettingsΔΟΥViewModel : BaseViewModel
    {
        UnitOfWork uow;

        public SettingsΔΟΥViewModel()
        {
            uow = new UnitOfWork();
            ΔΟΥ = new XPCollection<ΔΟΥ>(uow);
            Προσθήκη = new Command(Create);
        }

        public XPCollection<ΔΟΥ> ΔΟΥ { get; set; }
        private async void Create(object obj)
        {
            await Shell.Current.Navigation.PushAsync(new ΔΟΥDetailViewPage(null));
        }
        public void Save()
        {
            ΔΟΥ.Reload();
        }
        public ICommand Προσθήκη { set; get; }
    }
}

using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class SettingsΤρόποςΑποστολήςViewModel : BaseViewModel
    {
        UnitOfWork uow;
        public ICommand Προσθήκη { set; get; }
        public XPCollection<ΤρόποςΑποστολής> Τρόποςαποστολής { get; set; }

        public SettingsΤρόποςΑποστολήςViewModel()
        {
            uow = new UnitOfWork();
            Τρόποςαποστολής = new XPCollection<ΤρόποςΑποστολής>(uow);
            Προσθήκη = new Command(Create);
        }
        private async void Create(object obj)
        {
            await Shell.Current.Navigation.PushAsync(new ΤρόποςΑποστολήςDetailViewPage(null));
        }
        public void Save()
        {
            Τρόποςαποστολής.Reload();
        }
    }
}

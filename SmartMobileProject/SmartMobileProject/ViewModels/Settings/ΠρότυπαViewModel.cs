using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels.Settings
{
    class ΠρότυπαViewModel : BaseViewModel
    {
        UnitOfWork uow;

        public ΠρότυπαViewModel()
        {
            uow = new UnitOfWork();
            Πρότυπα = new XPCollection<Πρότυπα>(uow);
            Προσθήκη = new Command(Create);
        }
        public XPCollection<Πρότυπα> Πρότυπα { get; set; }
        private async void Create(object obj)
        {
            await Shell.Current.Navigation.PushAsync(new ΠρότυπαDetailPage(null));
        }
        public void Save()
        {
            Πρότυπα.Reload();
        }
        public Command Προσθήκη { set; get; }
    }
}

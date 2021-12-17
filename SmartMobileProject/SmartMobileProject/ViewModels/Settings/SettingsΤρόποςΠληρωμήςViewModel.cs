using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views;
using SmartMobileProject.Views.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class SettingsΤρόποςΠληρωμήςViewModel : BaseViewModel
    {
        UnitOfWork uow;
        public ICommand Προσθήκη { set; get; }
        public XPCollection<ΤρόποςΠληρωμής> Τρόποςπληρωμής { get; set; }

        public SettingsΤρόποςΠληρωμήςViewModel()
        {
            uow = new UnitOfWork();
            Τρόποςπληρωμής = new XPCollection<ΤρόποςΠληρωμής>(uow);
            Προσθήκη = new Command(Create);
        }
        private async void Create(object obj)
        {
            await Shell.Current.Navigation.PushAsync(new ΤρόποςΠληρωμήςDetailViewPage(null));
        }
        public void Save()
        {
            Τρόποςπληρωμής.Reload();
        }
    }
}

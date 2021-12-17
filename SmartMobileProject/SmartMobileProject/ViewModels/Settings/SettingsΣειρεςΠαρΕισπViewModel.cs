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
    class SettingsΣειρεςΠαρΕισπViewModel : BaseViewModel
    {
        UnitOfWork uow;

        public SettingsΣειρεςΠαρΕισπViewModel()
        {
            uow = new UnitOfWork();
            Σειρες = new XPCollection<ΣειρέςΠαραστατικώνΕισπράξεων>(uow);
            Προσθήκη = new Command(Create);
        }

        public XPCollection<ΣειρέςΠαραστατικώνΕισπράξεων> Σειρες { get; set; }
        private async void Create(object obj)
        {
            await Shell.Current.Navigation.PushAsync(new ΣειρέςΠαραστατικώνΕισπράξεωνDetailViewPage(null));
        }
        public void Save()
        {
            Σειρες.Reload();
        }
        public ICommand Προσθήκη { set; get; }
    }
}

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
    class SettingsPrintViewModel : BaseViewModel
    {
        UnitOfWork uow;

        public SettingsPrintViewModel()
        {
            uow = new UnitOfWork();
            Σειρες = new XPCollection<ΣειρέςΠαραστατικώνΠωλήσεων>(uow);
            Προσθήκη = new Command(Create);
        }

        public XPCollection<ΣειρέςΠαραστατικώνΠωλήσεων> Σειρες { get; set; }
        private async void Create(object obj)
        {
            var editForm = new ΣειρέςΠαραστατικώνΠωλήσεωνDetailViewPage(null);
            await Shell.Current.Navigation.PushAsync(editForm);
        }
        public void Save()
        {
            Σειρες.Reload();
        }
        public ICommand Προσθήκη { set; get; }
    }
}

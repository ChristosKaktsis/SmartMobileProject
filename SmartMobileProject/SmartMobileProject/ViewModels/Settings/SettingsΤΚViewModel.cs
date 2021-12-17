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
    class SettingsΤΚViewModel : BaseViewModel
    {
        UnitOfWork uow;

        public SettingsΤΚViewModel()
        {
            uow = new UnitOfWork();
            ΤΚ = new XPCollection<ΤαχυδρομικόςΚωδικός>(uow);
            Προσθήκη = new Command(Create);
        }

        public XPCollection<ΤαχυδρομικόςΚωδικός> ΤΚ { get; set; }
        private async void Create(object obj)
        {
            await Shell.Current.Navigation.PushAsync(new ΤΚDetailViewPage(null));
        }
        public void Save()
        {
            ΤΚ.Reload();
        }
        public ICommand Προσθήκη { set; get; }
    }
}

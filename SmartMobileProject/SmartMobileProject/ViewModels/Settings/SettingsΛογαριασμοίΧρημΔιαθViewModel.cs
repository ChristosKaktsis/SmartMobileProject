using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views.Settings;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class SettingsΛογαριασμοίΧρημΔιαθViewModel : BaseViewModel
    {
        UnitOfWork uow;

        public SettingsΛογαριασμοίΧρημΔιαθViewModel()
        {
            uow = new UnitOfWork();
            Λογαριασμοί = new XPCollection<ΛογαριασμοίΧρηματικώνΔιαθέσιμων>(uow);
            Προσθήκη = new Command(Create);
        }

        public XPCollection<ΛογαριασμοίΧρηματικώνΔιαθέσιμων> Λογαριασμοί { get; set; }
        private async void Create(object obj)
        {
            await Shell.Current.Navigation.PushAsync(new ΛογαριασμοίΧρημΔιαθDetailViewPage(null));
        }
        public void Save()
        {
            Λογαριασμοί.Reload();
        }
        public ICommand Προσθήκη { set; get; }
    }
}

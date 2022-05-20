using DevExpress.Xpo;
using SmartMobileProject.Models;
using SmartMobileProject.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            //Σειρες = new XPCollection<ΣειρέςΠαραστατικώνΠωλήσεων>(uow);
            Σειρες = new ObservableCollection<ΣειρέςΠαραστατικώνΠωλήσεων>();
            Προσθήκη = new Command(Create);
        }

        //public XPCollection<ΣειρέςΠαραστατικώνΠωλήσεων> Σειρες { get; set; }
        public ObservableCollection<ΣειρέςΠαραστατικώνΠωλήσεων> Σειρες { get; }
        private async void Create(object obj)
        {
            var editForm = new ΣειρέςΠαραστατικώνΠωλήσεωνDetailViewPage(null);
            await Shell.Current.Navigation.PushAsync(editForm);
        }
        public async void OnAppearing()
        {
            await LoadSeires();
        }

        private async Task LoadSeires()
        {
            try
            {
                var currentP = ((AppShell)Application.Current.MainPage).πωλητής;
                if (currentP == null)
                {
                    Console.WriteLine("LoadSeires Politis Is Null !!!!");
                    return ;
                }
                var items = uow.Query<ΣειρέςΠαραστατικώνΠωλήσεων>().Where(x=>x.IDΠωλητή == currentP.SmartOid);
                Σειρες.Clear();
                await Task.Run(() =>
                {
                    foreach(var item in items)
                    {
                        Σειρες.Add(item);
                    }
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void Save()
        {
            //Σειρες.Reload();
        }
        public ICommand Προσθήκη { set; get; }
    }
}

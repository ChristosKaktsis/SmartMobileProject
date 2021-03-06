using DevExpress.Xpo;
using SmartMobileProject.Core;
using SmartMobileProject.Models;
using SmartMobileProject.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
       public bool Remember { get => Preferences.Get("Remember", false); }

        UnitOfWork uow = ((App)Application.Current).uow;
        public XPCollection<Πωλητής> Πωλητές { get; set; }
        private Πωλητής πωλητής;
        public Πωλητής Πωλητής
        {
            get
            {
                return πωλητής;
            }
            set
            {
                πωλητής = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Πωλητής"));
            }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }
        public LoginViewModel()
        {
            
            SetAll();   
            LoginCommand = new Command(OnLoginClicked);
            ΝέοςΠωλητής = new Command(CreateNew);
            BackToSettingsCommand = new Command(async () => await AppShell.Current.GoToAsync($"//{nameof(PreLoginPage)}"));
        }
        public List<string> items { get; set; }
        private async void SetAll()
        {    
            await Task.Run(() =>
            {
                Πωλητές = new XPCollection<Πωλητής>(uow);
                if(Preferences.Get("Πωλητής", null)!=null)
                {
                    string politis = Preferences.Get("Πωλητής", null);
                    // do something with id
                    var find = uow.GetObjectByKey<Πωλητής>(int.Parse(politis));
                    Πωλητής = (Πωλητής)find;
                }
            });
        }
        private void CreateNew(object obj)
        {
            Πωλητής = new Πωλητής(uow);
            Πωλητής.Ονοματεπώνυμο = "Νέος Πωλητής";
            Πωλητής.SmartOid = Guid.NewGuid();
            if (uow.InTransaction)
            {
                try
                {
                    uow.CommitChanges();
                    LoginCommand.Execute(null);
                }
                catch { }
            }
        }

        public Action DisplayInvalidLoginPrompt;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
     
        public async void  OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
           // await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            if(Πωλητής == null)
            {
                return;
            }
            Preferences.Set("Remember", true);
            ((AppShell)Application.Current.MainPage).πωλητής = Πωλητής;
            await Shell.Current.GoToAsync($"//{nameof(ΠωλητήςViewPage)}");
        }
        public ICommand LoginCommand { protected set; get; }
        public ICommand ΝέοςΠωλητής { protected set; get; }
        public ICommand BackToSettingsCommand { get; set; }
    }
}

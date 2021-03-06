using SmartMobileProject.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class PreLoginViewModel : BaseViewModel
    {
        public PreLoginViewModel()
        {
            Συνέχεια = new Command(Continue);
        }

        private async void Continue(object obj)
        {
            
            if (Preferences.Get("Remember", false))
            {
               await Shell.Current.Navigation.PopAsync();             
                return;
            }
            
            AllLoading = true;
            if (OnlineMode)
            {             
                await XpoHelper.CreatePolitisData();
                await XpoHelper.CreateSTOIXEIAETAIRIASData();
            }               
            AllLoading = false;
            await AppShell.Current.GoToAsync("///LoginPage");
        }
 
        public string IP
        {
            get => Preferences.Get(nameof(IP), "79.129.5.42");
            set
            {
                Preferences.Set(nameof(IP), value);
                OnPropertyChanged(nameof(IP));
            }
        }
        
        //public string Port1
        //{
        //    get => Preferences.Get(nameof(Port1), "8881");
        //    set
        //    {
        //        Preferences.Set(nameof(Port1), value);
        //        OnPropertyChanged(nameof(Port1));
        //    }
        //}
        
        public string Port2
        {
            get => Preferences.Get(nameof(Port2), "8882");
            set
            {
                Preferences.Set(nameof(Port2), value);
                OnPropertyChanged(nameof(Port2));
            }
        }
        public string uname
        {
            get => Preferences.Get(nameof(uname), "DemoAdmin");
            set
            {
                Preferences.Set(nameof(uname), value);
                OnPropertyChanged(nameof(uname));
            }
        }
        public string passwrd
        {
            get => Preferences.Get(nameof(passwrd), "DemoPass");
            set
            {
                Preferences.Set(nameof(passwrd), value);
                OnPropertyChanged(nameof(passwrd));
            }
        }
        bool allLoading = false;
        public bool AllLoading
        {
            get { return allLoading; }
            set
            {
                SetProperty(ref allLoading, value);
            }
        }
        public Command Συνέχεια { set; get; }
    }
}

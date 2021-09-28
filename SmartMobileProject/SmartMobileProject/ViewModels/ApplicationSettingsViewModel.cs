using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ApplicationSettingsViewModel : BaseViewModel
    {
        public ApplicationSettingsViewModel()
        {
        }
        public bool OnlineMode
        {
            get => Preferences.Get(nameof(OnlineMode), false); 
            set
            {
                Preferences.Set(nameof(OnlineMode), value);
                OnPropertyChanged(nameof(OnlineMode));
            }
        }
    }
}

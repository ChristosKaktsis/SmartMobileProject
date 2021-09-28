using SmartMobileProject.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class LoadFromSmartModel : BaseViewModel
    {
        public bool OnlineMode
        {
            get => Preferences.Get(nameof(OnlineMode), false);
            set
            {
                Preferences.Set(nameof(OnlineMode), value);
                OnPropertyChanged(nameof(OnlineMode));
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
        bool animationPlaying = false;
        public bool AnimationPlaying
        {
            get { return animationPlaying; }
            set
            {
                SetProperty(ref animationPlaying, value);
            }
        }
        public LoadFromSmartModel()
        {
            Shell.Current.FlyoutIsPresented = false;    
            
        }
        public async Task<bool> LoadAll()
        {
            AllLoading = true;
            bool done = await XpoHelper.CreateTableData();
            AllLoading = false;
           
            return done;
        }
        public async Task<bool> LoadItems()
        {
            AllLoading = true;
            bool done = await XpoHelper.CreateEIDOSData();
            AllLoading = false;
           
            return done;
        }

        public async Task<bool> LoadAppointments()
        {
            AllLoading = true;
            bool done = await XpoHelper.CreateEvent();
            AllLoading = false;
            
            return done;
        }
        public async Task<bool> LoadPelates()
        {
            AllLoading = true;
            bool done = await XpoHelper.CreatePELATISData();
            bool done2 = await XpoHelper.CreateDIEUPELATIData();
            AllLoading = false;
            
            return done && done2;
        }

    }
}

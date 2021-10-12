using SmartMobileProject.Core;
using SmartMobileProject.Models;
using SmartMobileProject.Services;
using SmartMobileProject.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class UploadToSmartModel : BaseViewModel
    {
        bool allItemsLoading = false;
        public bool AllItemsLoading
        {
            get { return allItemsLoading; }
            set
            {
                SetProperty(ref allItemsLoading, value);
            }
        }
        bool pelatesItemsLoading = false;
        public bool PelatesItemsLoading
        {
            get { return pelatesItemsLoading; }
            set
            {
                SetProperty(ref pelatesItemsLoading, value);
            }
        }
        bool poliseisItemsLoading = false;
        public bool PoliseisItemsLoading
        {
            get { return poliseisItemsLoading; }
            set
            {
                SetProperty(ref poliseisItemsLoading, value);
            }
        }
        bool eispraxeisItemsLoading = false;
        public bool EispraxeisItemsLoading
        {
            get { return eispraxeisItemsLoading; }
            set
            {
                SetProperty(ref eispraxeisItemsLoading, value);
            }
        }
        bool ergasiaItemsLoading = false;
        public bool ErgasiaItemsLoading
        {
            get { return ergasiaItemsLoading; }
            set
            {
                SetProperty(ref ergasiaItemsLoading, value);
            }
        }
        bool energeiaItemsLoading = false;
        public bool EnergeiaItemsLoading
        {
            get { return energeiaItemsLoading; }
            set
            {
                SetProperty(ref energeiaItemsLoading, value);
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
        public UploadToSmartModel()
        {
            Shell.Current.FlyoutIsPresented = false;  
        }
        private async Task<bool> CallThisClass(IDataControl repository)
        {
           return await repository.UpdateItemAsync();
        }
        public async Task<bool> SendAllItems()
        {            
            AllItemsLoading = true;
            var done = await Task.WhenAll(CallThisClass(new RepositoryΠελάτες()),
                
                CallThisClass(new RepositoryΠαρΠωλήσεων()),
                CallThisClass(new RepositoryΠαρΕισπράξεων()),
                CallThisClass(new RepositoryΑξιόγραφο()),
                CallThisClass(new RepositoryΕργασία()),
                CallThisClass(new RepositoryΕνέργεια()));
                
            if (done.All(x => x))
            {
                AllItemsLoading = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> SendPelatesItems()
        {
           PelatesItemsLoading = true;
           var done = await CallThisClass(new RepositoryΠελάτες());         
            if (done)
            {
                PelatesItemsLoading = false;
                return true;
            }
            else
            {
                PelatesItemsLoading = false;
                return false;
            }
        }
        public async Task<bool> SendPoliseisItems()
        {
            PoliseisItemsLoading = true;
            var done = await Task.WhenAll(
                CallThisClass(new RepositoryΠαρΠωλήσεων()));
                
            if (done.All(x => x))
            {
                PoliseisItemsLoading = false;
                return true;
            }
            else
            {
                PoliseisItemsLoading = false;
                return false;
            }
        }
        public async Task<bool> SendEispraxeisItems()
        {
            EispraxeisItemsLoading = true;
            var done = await Task.WhenAll(
                CallThisClass(new RepositoryΠαρΕισπράξεων()),
                CallThisClass(new RepositoryΑξιόγραφο()));
                
            if (done.All(x => x))
            {
                EispraxeisItemsLoading = false;
                return true;
            }
            else
            {
                EispraxeisItemsLoading = false;
                return false;
            }
        }
    }
}

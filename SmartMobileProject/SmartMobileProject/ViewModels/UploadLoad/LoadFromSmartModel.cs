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
            await Task.WhenAll(
                XpoHelper.CreateOIKOGENEIAEIDOUSData(),
                XpoHelper.CreateOMADAEIDOUSData(),
                XpoHelper.CreateKATIGORIAEIDOUSData(),
                XpoHelper.CreateYPOOIKOGENEIAEIDOYSData()
                );
            bool done = await XpoHelper.CreateEIDOSData();
            bool barcode = await XpoHelper.CreateBarCodeEIDOSData();
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
            //return done ;
        }
        public async Task<bool> LoadBarCode()
        {
            AllLoading = true;
            bool barcode = await XpoHelper.CreateBarCodeEIDOSData();
            AllLoading = false;

            return barcode;
        }
        public async Task<bool> LoadImages()
        {
            try
            {
                AllLoading = true;
                await XpoHelper.LoadImages();
                AllLoading = false;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return true;
        }

    }
}

using SmartMobileProject.Core;
using SmartMobileProject.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartMobileProject.ViewModels
{
    class ActivationViewModel : BaseViewModel
    {

        bool popupon;
        public bool PopUpOn
        {
            get => popupon;
            set
            {
                SetProperty(ref popupon, value);
            }
        }
        string licenseText = "Δοκιμαστικό προϊόν";
        public string LicenseText
        {
            get => licenseText;
            set
            {
                SetProperty(ref licenseText, value);
            }
        }
        Color licenseColor = Color.Orange;
        public Color LicenseColor
        {
            get => licenseColor;
            set
            {
                SetProperty(ref licenseColor, value);
            }
        }
       
        ImageSource licenseImage = "important";
        public ImageSource LicenseImage
        {
            get => licenseImage;
            set
            {
                SetProperty(ref licenseImage, value);
            }
        }
        string activationCode;
        public string ActivationCode
        {
            get => activationCode;
            set
            {
                SetProperty(ref activationCode, value);
            }
        }
        public ActivationViewModel()
        {
            Shell.Current.FlyoutIsPresented = false;
            LicenseText += "\n" + "Απομένουν μέρες : " + Preferences.Get("DaysLeft", 0);
            if (Lock)
            {
                LicenseColor = Color.Red;
                LicenseImage = "expired";
                LicenseText = "Η δοκιμή έληξε";
            }else if (Active)
            {
                LicenseColor = Color.Green;
                LicenseImage = "checked150";
                LicenseText = "Ενεργοποιημένο προϊόν";
            }
            
        }
        public async void CheckActivationCode() 
        {
            //set the activation code 
            //check if valid
            //set Pref
            await ActivationCheck.CheckActivationCode(ActivationCode,GetId());
            if (Active)
            {
                LicenseColor = Color.Green;
                LicenseImage = "checked150";
                LicenseText = "Ενεργοποιημένο προϊόν";
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Activation",
                "Κάτι πήγε στραβά με το κλειδί ενεργοποίησης", "Εντάξει");
            }
        }

        private string GetId()
        {
            return DependencyService.Get<IDevice>().GetIdentifier();
        }
    }
}

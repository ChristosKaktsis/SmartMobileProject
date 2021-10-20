using SmartMobileProject.Core;
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
        string licenseText = "Trial Product";
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
            LicenseText += "\n" + "Days Left : " + Preferences.Get("DaysLeft", 0);
            if (Lock)
            {
                LicenseColor = Color.Red;
                LicenseImage = "expired";
                LicenseText = "Trial Expired";
            }else if (Active)
            {
                LicenseColor = Color.Green;
                LicenseImage = "checked150";
                LicenseText = "Product Activated";
            }
            
        }
        public async void CheckActivationCode() 
        {
            //set the activation code 
            //check if valid
            //set Pref
            await ActivationCheck.CheckActivationCode(ActivationCode);
            if (Active)
            {
                LicenseColor = Color.Green;
                LicenseImage = "checked150";
                LicenseText = "Product Activated";
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Activation",
                "Κάτι πήγε στραβά με το κλειδί ενεργοποίησης", "Εντάξει");
            }
        }
    }
}

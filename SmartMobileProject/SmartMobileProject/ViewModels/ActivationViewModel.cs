using System;
using System.Collections.Generic;
using System.Text;
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
        public bool Lock
        {
            get => Preferences.Get(nameof(Lock), false);
            set
            {
                OnPropertyChanged(nameof(Lock));
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
        public ActivationViewModel()
        {
            Shell.Current.FlyoutIsPresented = false;
            if(Lock)
            {
                LicenseColor = Color.Red;
                LicenseImage = "expired";
                LicenseText = "Triela Expired";
            }
        }

    }
}

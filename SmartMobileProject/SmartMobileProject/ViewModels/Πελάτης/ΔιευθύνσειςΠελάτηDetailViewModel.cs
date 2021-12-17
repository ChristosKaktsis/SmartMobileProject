using DevExpress.Xpo;
using SmartMobileProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace SmartMobileProject.ViewModels
{
    [QueryProperty(nameof(IsFromEdit), nameof(IsFromEdit))]
    class ΔιευθύνσειςΠελάτηDetailViewModel : BaseViewModel
    {
        App app = (App)Application.Current; //
        AppShell appShell = (AppShell)Application.Current.MainPage;
        public string name;
        public static ΔιευθύνσειςΠελάτη editAddress;
        private ΔιευθύνσειςΠελάτη address;
        public ΔιευθύνσειςΠελάτη Address
        {
            get
            {
                return address;
            }
            set
            {
                SetProperty(ref address, value);

            }
        }
        private ΤαχυδρομικόςΚωδικός tk;
        public ΤαχυδρομικόςΚωδικός Tk
        {
            get
            {
                return tk;
            }
            set
            {
                SetProperty(ref tk, value);
                Address.ΤΚ = value;
                Address.Περιοχή = value.Περιοχή;
                Address.Πόλη = value.Πόλη;
            }
        }
        public XPCollection<ΤαχυδρομικόςΚωδικός> TK { get; set; }
        public XPCollection<Πόλη> Poli { get; set; }
        public bool saveButtonPressed = false;
        public bool isFromEdit = false;
        public bool IsFromEdit
        {
            get
            {
                return isFromEdit;
            }
            set
            {
                isFromEdit = value;
                SetProperty(ref isFromEdit, value);
                SetTitle();
            }
        }

        public ΔιευθύνσειςΠελάτηDetailViewModel()
        {
            //Ονομα πελάτη για το pin στο χάρτη
            name = string.IsNullOrEmpty(appShell.customer1.DisplayName) ? "No displayName" : appShell.customer1.DisplayName;
            //
            TK = new XPCollection<ΤαχυδρομικόςΚωδικός>(app.uow);
            Poli = new XPCollection<Πόλη>(app.uow);
            if (editAddress==null)
            {
                Title = "Νέα Διεύθυνση";
                Address = new ΔιευθύνσειςΠελάτη(app.uow);
                Address.SmartOid = Guid.NewGuid();
                Address.ΗμνίαΔημ = DateTime.Now;
                GetCurrentLocation();
            }
            else
            {
                Address = editAddress;
                if(Address.ΤΚ != null)
                {
                    Tk = Address.ΤΚ;
                }
            }
           
            Αποθήκευση = new Command(Save);
        }

        private async void Save(object obj)
        {
            //ΓΠ και ΓΜ
            GetSelectedLocation();

            if (app.uow.InTransaction)
            {
                // app.uow.CommitChanges();
                Address.CanUpload = true;
                appShell.customer1.ΔιευθύνσειςΠελάτη.Add(Address);
                appShell.customer1.ΚεντρικήΔιευθυνση = Address;
               //Address.Customer = appShell.customer1;
            }
            //tell the app that the save button is pressed
            saveButtonPressed = true;

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private void SetTitle()
        {
            if (IsFromEdit)
            {
                Title = "Επεξεργασια Διεύθυνσης";
            }
            else
            {
                Title = "Νέα Διεύθυνση";
            }
        }

        Position position;
        public Position Position
        {
            get
            {
                return position;
            }
            set
            {
                SetProperty(ref position, value);

            }
        }
        async void GetSelectedLocation()
        {
            try
            {
                //get location with addres name 
                var address = Address.Οδός+" "+Address.Αριθμός+", "+Address.ΤΚ.Περιοχή+" "+Address.ΤΚ.Ονοματκ+", "+Address.ΤΚ.Χώρα;
                var locations = await Geocoding.GetLocationsAsync(address);
                var location = locations?.FirstOrDefault();
                if (location != null)
                {
                    Position = new Position(location.Latitude, location.Longitude);
                    Address.ΓεωγραφικόΠλάτος = location.Latitude;
                    Address.ΓεωγραφικόΜήκος = location.Longitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
        async void GetCurrentLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Position = new Position(location.Latitude, location.Longitude);
                    Address.ΓεωγραφικόΠλάτος = location.Latitude;
                    Address.ΓεωγραφικόΜήκος = location.Longitude;
                }
                var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    var post = placemark.PostalCode.Replace(" ", "");
                    Tk = app.uow.Query<ΤαχυδρομικόςΚωδικός>().Where(x => x.Ονοματκ == post).FirstOrDefault();
                    Address.Αριθμός = placemark.SubThoroughfare;
                    Address.Οδός = string.IsNullOrEmpty(placemark.Thoroughfare)? placemark.Locality : placemark.Thoroughfare;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
        public ICommand Αποθήκευση { set; get; }
    }
}
